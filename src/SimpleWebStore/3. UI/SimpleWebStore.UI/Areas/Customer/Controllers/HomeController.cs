using Microsoft.AspNetCore.Mvc;
using SimpleWebStore.DAL.UnitOfWork;
using SimpleWebStore.Domain.Entities;
using SimpleWebStore.UI.ViewModels;

namespace BookWebStore.UI.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IUnitOfWork unitOfWork,
            ILogger<HomeController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {
            IEnumerable<Product> productList = await _unitOfWork.ProductRepository.GetAllEntitiesAsync(
                includeProps: "Category,CoverType");

            return View(productList);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            ShoppingCart cartObj = new()
            {
                Count = 1,
                Product = await _unitOfWork.ProductRepository.GetEntityAsync(c => c.Id == id,
                    includeProps: "Category,CoverType")
            };

            return View(cartObj);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}