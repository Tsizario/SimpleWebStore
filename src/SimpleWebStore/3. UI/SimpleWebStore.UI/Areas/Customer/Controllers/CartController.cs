using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SimpleWebStore.DAL.UnitOfWork;
using SimpleWebStore.UI.Areas.Admin.Controllers;
using SimpleWebStore.UI.Controllers;

namespace SimpleWebStore.UI.Areas.Customer.Controllers
{
    [Area("Customer")]
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

        public async Task<ActionResult> Index()
        {
            return View();
        }
    }
}
