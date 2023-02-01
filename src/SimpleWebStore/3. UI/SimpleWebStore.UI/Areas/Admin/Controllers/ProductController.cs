using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SimpleWebStore.DAL.UnitOfWork;
using SimpleWebStore.Domain.Constants;
using SimpleWebStore.Domain.Entities;
using SimpleWebStore.UI.Areas.Abstractions;
using SimpleWebStore.UI.ViewModels;
using System.Linq;

namespace SimpleWebStore.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : GeneralController
    {
        public ProductController(IUnitOfWork unitOfWork,
            ILogger<CategoryController> logger,
            INotyfService toastNotification)
                : base(unitOfWork, logger, toastNotification)
        {
        }

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
        public async Task<ActionResult> Upsert(Guid? id)
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllEntitiesAsync();
            var coverTypes = await _unitOfWork.CoverTypeRepository.GetAllEntitiesAsync();

            ProductViewModel productViewModel = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.CategoryRepository
                    .GetAllEntitiesAsync().Result.Select(item =>
                        new SelectListItem
                        {
                            Text = item.Name,
                            Value = item.Id.ToString()
                        }),
                CoverTypeList = _unitOfWork.CoverTypeRepository
                    .GetAllEntitiesAsync().Result.Select(item =>
                        new SelectListItem
                        {
                            Text = item.Name,
                            Value = item.Id.ToString()
                        }),
            };

            if (id == null || id == Guid.Empty)
            {
                return View(productViewModel);
            }
            else
            {

            }

            return View(productViewModel);
        }

        // PUT
        [HttpPost]
        [ActionName("Upsert")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upsert(CoverType coverType)
        {
            if (!ModelState.IsValid)
            {
                return View(coverType);
            }

            var updatedItem = await _unitOfWork.CoverTypeRepository.UpdateEntityAsync(coverType);

            if (updatedItem == null)
            {
                _toastNotification.Error(Errors.CoverTypeDoesNotExist);

                return RedirectToAction(nameof(Upsert), updatedItem);
            }

            await _unitOfWork.SaveAsync();

            _toastNotification.Success(Notifications.CoverTypeCreateSuccess);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return NotFound();
            }

            var category = await _unitOfWork.CoverTypeRepository.GetEntityAsync(c => c.Id == id);

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
            var deletedItem = await _unitOfWork.CoverTypeRepository.RemoveEntityAsync(id);

            if (deletedItem == null)
            {
                _toastNotification.Error(Errors.CoverTypeDoesNotExist);

                return View(nameof(Index));
            }

            await _unitOfWork.SaveAsync();

            _toastNotification.Success(Notifications.CoverTypeDeleteSuccess);

            return RedirectToAction(nameof(Index));
        }
    }
}
