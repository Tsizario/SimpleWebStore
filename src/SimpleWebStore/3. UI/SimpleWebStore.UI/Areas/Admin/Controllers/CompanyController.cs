using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SimpleWebStore.DAL.UnitOfWork;
using SimpleWebStore.Domain.Constants;
using SimpleWebStore.Domain.Entities;
using SimpleWebStore.UI.Controllers;

namespace SimpleWebStore.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : GeneralController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(IWebHostEnvironment webHostEnvironment,
            IUnitOfWork unitOfWork,
            ILogger<CompanyController> logger,
            INotyfService toastNotification)
                : base(unitOfWork, toastNotification)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            //var allCoverTypes = await _unitOfWork.ProductRepository.GetAllEntitiesAsync();

            //if (allCoverTypes == null)
            //{
            //    _toastNotification.Error(Errors.CategorySameNumber);
            //}

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Upsert(Guid? id)
        {
            Company company = new();            

            if (id == null || id == Guid.Empty)
            {
                return View(company);
            }
            else
            {
                company = await _unitOfWork.CompanyRepository.GetEntityAsync(p => p.Id == id);

                return View(company);
            }
        }

        // PUT
        [HttpPost]
        [ActionName("Upsert")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upsert(Company company, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                return View(company);
            }

            string notification = null;

            if (company.Id == Guid.Empty)
            {
                await _unitOfWork.CompanyRepository.AddEntityAsync(company);

                notification = Notifications.CompanytCreateSuccess;
            }
            else
            {
                await _unitOfWork.CompanyRepository.UpdateEntityAsync(company);

                notification = Notifications.CompanyUpdateSuccess;
            }

            await _unitOfWork.SaveAsync();

            _toastNotification.Success(notification);

            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedItem = await _unitOfWork.CompanyRepository.GetEntityAsync(c => c.Id == id);

            if (deletedItem == null)
            {
                return Json(new { success = false, message = Errors.CompanyDeletingError });
            }

            await _unitOfWork.CompanyRepository.RemoveEntityAsync(deletedItem);

            await _unitOfWork.SaveAsync();

            return Json(new { success = true, message = Notifications.CompanyDeleteSuccess });
        }

        #region API Calls

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var companyList = await _unitOfWork.CompanyRepository.GetAllEntitiesAsync();

            return Json(new { data = companyList });
        }

        #endregion
    }
}