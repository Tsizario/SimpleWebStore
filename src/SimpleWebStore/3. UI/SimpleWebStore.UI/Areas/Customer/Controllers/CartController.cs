using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleWebStore.DAL.UnitOfWork;
using SimpleWebStore.UI.Controllers;
using SimpleWebStore.UI.ViewModels;
using System.Security.Claims;

namespace SimpleWebStore.UI.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : GeneralController
    {
        private readonly ILogger<CartController> _logger;

        public CartController(IUnitOfWork unitOfWork,
            ILogger<CartController> logger,
            INotyfService toastNotification)
                : base(unitOfWork, toastNotification)
        {
            _logger = logger;
        }

        public ShoppingCartViewModel ShoppingCartViewModel { get; set; }

        public int OrderTotal { get; set; }

        public async Task<ActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity!.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartViewModel = new ShoppingCartViewModel()
            {
                ListCart = _unitOfWork.ShoppingCartRepository.GetAllEntitiesAsync(
                    u => u.AppUserId == claim.Value,
                    includeProps: "Product").Result
            };

            foreach(var cart in ShoppingCartViewModel.ListCart)
            {
                cart.ItemPrice = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price,
                    cart.Product.Price50, cart.Product.Price100);

                ShoppingCartViewModel.CartTotal += (cart.ItemPrice * cart.Count);
            }

            return View(ShoppingCartViewModel);
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
