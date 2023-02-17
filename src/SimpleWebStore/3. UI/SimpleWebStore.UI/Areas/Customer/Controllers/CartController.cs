using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleWebStore.DAL.UnitOfWork;
using SimpleWebStore.Domain.Constants;
using SimpleWebStore.UI.Controllers;
using SimpleWebStore.UI.ViewModels;
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

        public ShoppingCartViewModel ShoppingCartViewModel { get; set; }

        public int OrderTotal { get; set; }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity!.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartViewModel = new ShoppingCartViewModel()
            {
                ListCart = _unitOfWork.ShoppingCartRepository.GetAllEntitiesAsync(
                    u => u.AppUserId == claim.Value,
                    includeProps: "Product").Result,
                OrderHeader = new()
            };

            foreach(var cart in ShoppingCartViewModel.ListCart)
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
                ListCart = _unitOfWork.ShoppingCartRepository.GetAllEntitiesAsync(
                    u => u.AppUserId == claim.Value,
                    includeProps: "Product").Result,
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


        public async Task<ActionResult> Plus(Guid cartId)
        {
            var cart = await _unitOfWork.ShoppingCartRepository.GetEntityAsync(u => u.Id == cartId);

            _unitOfWork.ShoppingCartRepository.IncrementCount(cart, 1);

            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

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
