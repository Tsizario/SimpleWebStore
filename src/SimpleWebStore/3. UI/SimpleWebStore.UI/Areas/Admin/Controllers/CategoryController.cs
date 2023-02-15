using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SimpleWebStore.DAL.UnitOfWork;
using SimpleWebStore.Domain.Constants;
using SimpleWebStore.Domain.Entities;
using SimpleWebStore.UI.Controllers;

namespace SimpleWebStore.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : GeneralController
    {
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(IUnitOfWork unitOfWork,
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
            var allCategories = await _unitOfWork.CategoryRepository.GetAllEntitiesAsync();

            if (allCategories == null)
            {
                _toastNotification.Error(Errors.CategorySameNumber);
            }

            return View(allCategories);
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
        public async Task<ActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", Errors.CategorySameNumber);

                return View(category);
            }

            var addedItem = await _unitOfWork.CategoryRepository.AddEntityAsync(category);

            if (addedItem == null)
            {
                _toastNotification.Error(Errors.CategoryAddingError);

                return RedirectToAction(nameof(Index));
            }

            await _unitOfWork.SaveAsync();

            _toastNotification.Success(Notifications.CategoryCreateSuccess);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return NotFound();
            }

            var category = await _unitOfWork.CategoryRepository.GetEntityAsync(e => e.Id == id);

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
        public async Task<ActionResult> Edit(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            if (category.Name == category.DisplayOrder.ToString())
            {
                // set the same name as on view/page (for example, 'Name' property of model as here)
                ModelState.AddModelError("Name", Errors.CategorySameNumber);

                return View(category);
            }

            var updatedItem = await _unitOfWork.CategoryRepository.UpdateEntityAsync(category);

            if (updatedItem == null)
            {
                _toastNotification.Error(Errors.CategoryDoesNotExist);

                return RedirectToAction(nameof(Edit), updatedItem);
            }

            await _unitOfWork.SaveAsync();

            _toastNotification.Success(Notifications.CategoryUpdateSuccess);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return NotFound();
            }

            var category = await _unitOfWork.CategoryRepository.GetEntityAsync(e => e.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // DELETE
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid id)
        {
            var deletedItem = await _unitOfWork.CategoryRepository.GetEntityAsync(c => c.Id == id);

            if (deletedItem == null)
            {
                _toastNotification.Error(Errors.CategoryDoesNotExist);

                return View(nameof(Index));
            }

            await _unitOfWork.CategoryRepository.RemoveEntityAsync(deletedItem);

            await _unitOfWork.SaveAsync();

            _toastNotification.Success(Notifications.CategoryDeleteSuccess);

            return RedirectToAction(nameof(Index));
        }
    }
}
