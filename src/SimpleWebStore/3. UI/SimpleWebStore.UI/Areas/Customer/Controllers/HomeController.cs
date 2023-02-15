using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleWebStore.DAL.UnitOfWork;
using SimpleWebStore.Domain.Entities;
using SimpleWebStore.UI.Controllers;
using System.Security.Claims;

namespace BookWebStore.UI.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : GeneralController
    {
        public HomeController(IUnitOfWork unitOfWork, INotyfService toastNotification)
            : base(unitOfWork, toastNotification)
        {
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> productList = await _unitOfWork.ProductRepository.GetAllEntitiesAsync(
                includeProps: "Category,CoverType");

            return View(productList);
        }

        [Authorize]
        public async Task<IActionResult> Details(Guid? productId)
        {
            ShoppingCart cartObj = new()
            {
                ProductId = productId.Value,
                Product = await _unitOfWork.ProductRepository.GetEntityAsync(c => c.Id == productId,
                    includeProps: "Category,CoverType"),

                Count = 1,
            };

            return View(cartObj);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity!.FindFirst(ClaimTypes.NameIdentifier);

            shoppingCart.AppUserId = claim!.Value;

            var cartFromDb = await _unitOfWork.ShoppingCartRepository
                .GetEntityAsync(cart => cart.AppUserId == claim.Value 
                    && cart.ProductId == shoppingCart.ProductId);

            if (cartFromDb == null)
            {
                await _unitOfWork.ShoppingCartRepository.AddEntityAsync(shoppingCart);
            }
            else
            {
                _unitOfWork.ShoppingCartRepository.IncrementCount(cartFromDb, shoppingCart.Count);
            }

            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}