using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SimpleWebStore.DAL.UnitOfWork;
using SimpleWebStore.UI.Areas.Admin.Controllers;

namespace SimpleWebStore.UI.Areas.Abstractions
{
    public class GeneralController : Controller
    {
        private protected readonly IUnitOfWork _unitOfWork;
        private protected readonly ILogger<CategoryController> _logger;
        private protected readonly INotyfService _toastNotification;

        public GeneralController(IUnitOfWork unitOfWork,
            INotyfService toastNotification)
        {
            _unitOfWork = unitOfWork;
            _toastNotification = toastNotification;
        }
    }
}
