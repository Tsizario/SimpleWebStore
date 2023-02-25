using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using SimpleWebStore.DAL.UnitOfWork;
using SimpleWebStore.Domain.Constants;
using SimpleWebStore.Domain.Entities;
using SimpleWebStore.UI.Controllers;
using SimpleWebStore.UI.ViewModels;
using Stripe.Checkout;
using System.Security.Claims;

namespace SimpleWebStore.UI.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : GeneralController
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CartController> _logger;

        public CartController(IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<CartController> logger,
            INotyfService toastNotification)
                : base(unitOfWork, toastNotification)
        {
            _mapper = mapper;
            _logger = logger;
        }

        [BindProperty]
        public ShoppingCartViewModel ShoppingCartViewModel { get; set; }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity!.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartViewModel = new ShoppingCartViewModel()
            {
                ListCart = await _unitOfWork.ShoppingCartRepository.GetAllEntitiesAsync(
                    u => u.AppUserId == claim.Value,
                    includeProps: "Product"),
                OrderHeader = new()
            };

            foreach (var cart in ShoppingCartViewModel.ListCart)
            {
                cart.ItemPrice = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price,
                    cart.Product.Price50, cart.Product.Price100);

                ShoppingCartViewModel.OrderHeader.OrderTotal += (cart.ItemPrice * cart.Count);
            }

            return View(ShoppingCartViewModel);
        }

        [HttpGet]
        public async Task<ActionResult> Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity!.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartViewModel = new ShoppingCartViewModel()
            {
                ListCart = await _unitOfWork.ShoppingCartRepository.GetAllEntitiesAsync(
                    u => u.AppUserId == claim.Value,
                    includeProps: "Product"),
                OrderHeader = new()
                {
                    AppUser = _unitOfWork.AppUserRepository.GetEntityAsync(u => u.Id == claim.Value).Result,
                }
            };

            var orderHeader = ShoppingCartViewModel.OrderHeader;
            var userInfo = ShoppingCartViewModel.OrderHeader.AppUser;

            _mapper.Map(userInfo, orderHeader);

            foreach (var cart in ShoppingCartViewModel.ListCart)
            {
                cart.ItemPrice = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price,
                    cart.Product.Price50, cart.Product.Price100);

                ShoppingCartViewModel.OrderHeader.OrderTotal += (cart.ItemPrice * cart.Count);
            }

            return View(ShoppingCartViewModel);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SummaryPost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity!.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartViewModel.ListCart = await _unitOfWork.ShoppingCartRepository.GetAllEntitiesAsync(
                    u => u.AppUserId == claim.Value,
                    includeProps: "Product");

            ShoppingCartViewModel.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartViewModel.OrderHeader.AppUserId = claim.Value;

            foreach (var cart in ShoppingCartViewModel.ListCart)
            {
                cart.ItemPrice = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price,
                    cart.Product.Price50, cart.Product.Price100);

                ShoppingCartViewModel.OrderHeader.OrderTotal += (cart.ItemPrice * cart.Count);
            }

            var appUser = await _unitOfWork.AppUserRepository.GetEntityAsync(
                u => u.Id == ShoppingCartViewModel.OrderHeader.AppUserId);

            if (appUser.CompanyId.GetValueOrDefault() != Guid.Empty)
            {
                ShoppingCartViewModel.OrderHeader.PaymentStatus = PaymentStatuses.PaymentStatusDelayedPayment;
                ShoppingCartViewModel.OrderHeader.OrderStatus = Statuses.StatusApproved;
            }
            else
            {
                ShoppingCartViewModel.OrderHeader.PaymentStatus = PaymentStatuses.PaymentStatusPending;
                ShoppingCartViewModel.OrderHeader.OrderStatus = Statuses.StatusPending;
            }

            await _unitOfWork.OrderHeaderRepository.AddEntityAsync(ShoppingCartViewModel.OrderHeader);

            foreach (var cart in ShoppingCartViewModel.ListCart)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderId = ShoppingCartViewModel.OrderHeader.Id,
                    Price = cart.ItemPrice,
                    Count = cart.Count,
                };

                await _unitOfWork.OrderDetailRepository.AddEntityAsync(orderDetail);
            }

            await _unitOfWork.SaveAsync();

            if (ShoppingCartViewModel.OrderHeader.AppUser.CompanyId.GetValueOrDefault() == Guid.Empty)
            {
                //stripe settings
                var domain = "https://localhost:7180/";
                var options = new SessionCreateOptions
                {
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={ShoppingCartViewModel.OrderHeader.Id}",
                    CancelUrl = domain + $"customer/cart/Index",
                };

                foreach (var item in ShoppingCartViewModel.ListCart)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.ItemPrice * 100),
                            Currency = "uah",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Title
                            },
                        },
                        Quantity = item.Count,
                    };

                    options.LineItems.Add(sessionLineItem);
                }

                var service = new SessionService();
                Session session = service.Create(options);

                await _unitOfWork.OrderHeaderRepository.UpdateStripePaymentId(
                    ShoppingCartViewModel.OrderHeader.Id,
                    session.Id,
                    session.PaymentIntentId);

                await _unitOfWork.SaveAsync();

                Response.Headers.Add("Location", session.Url);

                return new StatusCodeResult(303);
            }

            return RedirectToAction("OrderConfirmation", "Cart",
                new { id = ShoppingCartViewModel.OrderHeader.Id });
        }

        public async Task<ActionResult> OrderConfirmation(Guid id)
        {
            OrderHeader orderHeader = await _unitOfWork.OrderHeaderRepository.GetEntityAsync(u => u.Id == id);

            if (orderHeader.PaymentStatus != PaymentStatuses.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    await _unitOfWork.OrderHeaderRepository.UpdateStatus(id,
                        Statuses.StatusApproved, PaymentStatuses.PaymentStatusApproved);

                    await _unitOfWork.SaveAsync();
                }
            }

            List<ShoppingCart> shoppingCarts = await _unitOfWork.ShoppingCartRepository.GetAllEntitiesAsync(
                u => u.AppUserId == orderHeader.AppUserId);

            await _unitOfWork.ShoppingCartRepository.RemoveEntitiesAsync(shoppingCarts);

            await _unitOfWork.SaveAsync();

            //_toastNotification.Success(Notifications);

            return View(id);
        }

        [HttpGet]
        public async Task<ActionResult> Plus(Guid cartId)
        {
            var cart = await _unitOfWork.ShoppingCartRepository.GetEntityAsync(u => u.Id == cartId);

            _unitOfWork.ShoppingCartRepository.IncrementCount(cart, 1);

            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ActionResult> Minus(Guid cartId)
        {
            var cart = await _unitOfWork.ShoppingCartRepository.GetEntityAsync(u => u.Id == cartId);

            if (cart.Count <= 1)
            {
                await _unitOfWork.ShoppingCartRepository.RemoveEntityAsync(cart);

                _toastNotification.Success(Notifications.ProductDeleteSuccess);
            }
            else
            {
                _unitOfWork.ShoppingCartRepository.DecrementCount(cart, 1);
            }

            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ActionResult> Remove(Guid cartId)
        {
            var cart = await _unitOfWork.ShoppingCartRepository.GetEntityAsync(u => u.Id == cartId);

            await _unitOfWork.ShoppingCartRepository.RemoveEntityAsync(cart);

            await _unitOfWork.SaveAsync();

            _toastNotification.Success(Notifications.ProductDeleteSuccess);

            return RedirectToAction(nameof(Index));
        }

        private double GetPriceBasedOnQuantity(double quantity,
            double price, double price50, double price100)
        {
            var unsignedDouble = Math.Abs(quantity);

            if (unsignedDouble >= 0 && unsignedDouble <= 50)
                return price;

            if (unsignedDouble >= 51 && unsignedDouble <= 100)
                return price50;

            return price100;
        }
    }
}
