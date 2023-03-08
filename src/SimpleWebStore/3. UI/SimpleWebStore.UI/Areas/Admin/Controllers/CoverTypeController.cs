using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SimpleWebStore.DAL.UnitOfWork;
using SimpleWebStore.Domain.Constants;
using SimpleWebStore.Domain.Entities;
using SimpleWebStore.UI.Controllers;

namespace SimpleWebStore.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : GeneralController
    {
        private readonly ILogger<CategoryController> _logger;

        public CoverTypeController(IUnitOfWork unitOfWork,
            ILogger<CategoryController> logger,
            INotyfService toastNotification)
                : base(unitOfWork, toastNotification)
        {
            _logger = logger;
        }

        // GET
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var allCoverTypes = await _unitOfWork.CoverTypeRepository.GetAllEntitiesAsync();

            if (allCoverTypes == null)
            {
                _toastNotification.Error(Errors.CategorySameNumber);
            }

            return View(allCoverTypes);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CoverType coverType)
        {
            if (!ModelState.IsValid)
            {
                return View(coverType);
            }

            var addedItem = await _unitOfWork.CoverTypeRepository.AddEntityAsync(coverType);

            if (addedItem == null)
            {
                _toastNotification.Error(Errors.CategoryAddingError);

                return RedirectToAction(nameof(Index));
            }

            await _unitOfWork.SaveAsync();

            _toastNotification.Success(Notifications.CoverTypeCreateSuccess);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return NotFound();
            }

            var category = await _unitOfWork.CoverTypeRepository.GetEntityAsync(e => e.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // PUT
        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CoverType coverType)
        {
            if (!ModelState.IsValid)
            {
                return View(coverType);
            }

            var updatedItem = await _unitOfWork.CoverTypeRepository.UpdateEntityAsync(coverType);

            if (updatedItem == null)
            {
                _toastNotification.Error(Errors.CoverTypeDoesNotExist);

                return RedirectToAction(nameof(Edit), updatedItem);
            }

            await _unitOfWork.SaveAsync();

            _toastNotification.Success(Notifications.CoverTypeCreateSuccess);

            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        [ActionName("Delete")]
        public async Task<ActionResult> Delete(Guid? id)
        {
            var deletedItem = await _unitOfWork.CoverTypeRepository.GetEntityAsync(u => u.Id == id);

            if (deletedItem == null)
            {
                return Json(new { success = false, message = Errors.CoverTypeAddingError });
            }

            await _unitOfWork.CoverTypeRepository.RemoveEntityAsync(deletedItem);

            await _unitOfWork.SaveAsync();

            return Json(new { success = true, message = Notifications.CoverTypeDeleteSuccess });
        }        

        #region API Calls

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var companyList = await _unitOfWork.CoverTypeRepository.GetAllEntitiesAsync();

            return Json(new { data = companyList });
        }

        #endregion
    }
}