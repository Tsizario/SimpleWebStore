using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleWebStore.DAL.UnitOfWork;
using SimpleWebStore.Domain.Constants;
using SimpleWebStore.Domain.Entities;
using SimpleWebStore.UI.Controllers;
using System.Security.Claims;

namespace SimpleWebStore.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : GeneralController
    {
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
                default:
                    statusToTheBase = null;
                    break;
            }

            if (User.IsInRole(Roles.Role_Admin) || User.IsInRole(Roles.Role_Employee))
            {   
                orderHeadersFromDb = await _unitOfWork.OrderHeaderRepository
                    .GetAllEntitiesAsync(includeProps: "AppUser");                
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
                selectedOrderHeaders = orderHeadersFromDb.Where(s => s.PaymentStatus == statusToTheBase).ToList();
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
