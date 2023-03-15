using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleWebStore.DAL.UnitOfWork;
using SimpleWebStore.Domain.Constants;
using SimpleWebStore.Domain.Entities;
using SimpleWebStore.UI.Controllers;
using SimpleWebStore.UI.ViewModels;
using Stripe;
using System.Security.Claims;

namespace SimpleWebStore.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : GeneralController
    {
        [BindProperty]
        public OrderViewModel OrderVM { get; set; }

        public OrderController(
            IUnitOfWork unitOfWork, 
            INotyfService toastNotification) 
                : base(unitOfWork, toastNotification)
        {
        }

        public async Task<ActionResult> Index()
        {
            return View();
        }

        public async Task<ActionResult> Details(Guid? orderId)
        {
            OrderVM = new OrderViewModel()
            {
                OrderHeader = _unitOfWork.OrderHeaderRepository
                    .GetEntityAsync(u => u.Id == orderId, includeProps: "AppUser").Result,
                OrderDetails = _unitOfWork.OrderDetailRepository
                    .GetAllEntitiesAsync(u => u.OrderId == orderId, includeProps: "Product").Result
            };

            return View(OrderVM);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Role_Admin + "," + Roles.Role_Employee)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateOrderDetail()
        {
            var orderHeaderToUpdate = new OrderHeader()
            {
                Id = OrderVM.OrderHeader.Id,
                Name = OrderVM.OrderHeader.Name,
                PhoneNumber = OrderVM.OrderHeader.PhoneNumber,
                Address = OrderVM.OrderHeader.Address,
                OrderDate = OrderVM.OrderHeader.OrderDate,
                City = OrderVM.OrderHeader.City,
                State = OrderVM.OrderHeader.State,
                PostalCode = OrderVM.OrderHeader.PostalCode
            };

            if (OrderVM.OrderHeader.Carrier != null)
            {
                orderHeaderToUpdate.Carrier = OrderVM.OrderHeader.Carrier;
            }

            if (OrderVM.OrderHeader.TrackingNumber != null)
            {
                orderHeaderToUpdate.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            }

            var orderHeaderFromDb = await _unitOfWork.OrderHeaderRepository.UpdateEntityAsync(orderHeaderToUpdate);

            await _unitOfWork.SaveAsync();

            _toastNotification.Success(Notifications.OrderHeaderUpdateSuccess);

            return RedirectToAction("Details", "Order", new { orderId = orderHeaderFromDb.Id });
        }

        [HttpPost]
        [Authorize(Roles = Roles.Role_Admin + "," + Roles.Role_Employee)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StartProcessing()
        {
            await _unitOfWork.OrderHeaderRepository.UpdateStatus(OrderVM.OrderHeader.Id,
                Statuses.StatusInProcess);

            await _unitOfWork.SaveAsync();

            _toastNotification.Success(Notifications.OrderStatusUpdateSuccess);

            return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = Roles.Role_Admin + "," + Roles.Role_Employee)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ShipOrder()
        {
            var orderHeader = await _unitOfWork.OrderHeaderRepository.GetEntityAsync(c => c.Id == OrderVM.OrderHeader.Id);

            var orderHeaderToUpdate = new OrderHeader()
            {
                Id = OrderVM.OrderHeader.Id,
                TrackingNumber = OrderVM.OrderHeader.TrackingNumber,
                Carrier = OrderVM.OrderHeader.Carrier,
                OrderStatus = Statuses.StatusShipped,
                ShippingDate = DateTime.Now
            };

            if (orderHeader.PaymentStatus == PaymentStatuses.PaymentStatusDelayedPayment)
            {
                orderHeaderToUpdate.PaymentDueDate = DateTime.Now.AddDays(30);
            }

            await _unitOfWork.OrderHeaderRepository.UpdateEntityAsync(orderHeaderToUpdate);

            await _unitOfWork.SaveAsync();

            _toastNotification.Success(Notifications.OrderShipSuccess);

            return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = Roles.Role_Admin + "," + Roles.Role_Employee)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CancelOrder()
        {
            var orderFromDb = await _unitOfWork.OrderHeaderRepository.GetEntityAsync(
                u => u.Id == OrderVM.OrderHeader.Id, tracked: false);

            if (orderFromDb.PaymentStatus == PaymentStatuses.PaymentStatusPending)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderFromDb.PaymentIntentId
                };

                var service = new RefundService();
                Refund refund = service.Create(options);

                await _unitOfWork.OrderHeaderRepository.UpdateStatus(orderFromDb.Id,
                    Statuses.StatusCancelled, PaymentStatuses.PaymentStatusRefunded);
            }
            else
            {
                await _unitOfWork.OrderHeaderRepository.UpdateStatus(orderFromDb.Id,
                    Statuses.StatusCancelled, PaymentStatuses.PaymentStatusCancelled);
            }
            
            await _unitOfWork.SaveAsync();

            _toastNotification.Success(Notifications.OrderCancelSuccess);

            return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
        }

        #region API Calls

        [HttpGet]
        public async Task<IActionResult> GetAll(string status)
        {
            List<OrderHeader> orderHeadersFromDb, selectedOrderHeaders = new List<OrderHeader>();
            var statusToTheBase = "";

            switch (status)
            {
                case "pending":
                    statusToTheBase = PaymentStatuses.PaymentStatusDelayedPayment;
                    break;
                case "inprocess":
                    statusToTheBase = Statuses.StatusInProcess;
                    break;
                case "completed":
                    statusToTheBase = Statuses.StatusShipped;
                    break;
                case "approved":
                    statusToTheBase = Statuses.StatusApproved;
                    break;
                case "all":
                    statusToTheBase = null;
                    break;
            }

            if (User.IsInRole(Roles.Role_Admin) || User.IsInRole(Roles.Role_Employee))
            { 
                if (statusToTheBase == PaymentStatuses.PaymentStatusDelayedPayment)
                {
                    orderHeadersFromDb = await _unitOfWork.OrderHeaderRepository
                        .GetAllEntitiesAsync(st => st.PaymentStatus == statusToTheBase,
                            includeProps: "AppUser");
                }
                else
                {
                    orderHeadersFromDb = await _unitOfWork.OrderHeaderRepository
                        .GetAllEntitiesAsync(includeProps: "AppUser");
                }
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                orderHeadersFromDb = await _unitOfWork.OrderHeaderRepository
                    .GetAllEntitiesAsync(u => u.AppUserId == claim.Value,
                        includeProps: "AppUser");
            }

            if (statusToTheBase != null)
            {
                selectedOrderHeaders = orderHeadersFromDb.Where(s => s.OrderStatus == statusToTheBase).ToList();
            }
            else
            {
                selectedOrderHeaders = orderHeadersFromDb;
            }

            return Json(new { data = selectedOrderHeaders });
        }

        #endregion
    }
}
