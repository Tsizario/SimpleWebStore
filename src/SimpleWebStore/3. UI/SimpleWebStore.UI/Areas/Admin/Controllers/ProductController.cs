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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IWebHostEnvironment webHostEnvironment,
            IUnitOfWork unitOfWork,
            ILogger<CategoryController> logger,
            INotyfService toastNotification)
                : base(unitOfWork, logger, toastNotification)
        {
            _webHostEnvironment = webHostEnvironment;
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
                productViewModel.Product = await _unitOfWork.ProductRepository.GetEntityAsync(p => p.Id == id);

                return View(productViewModel);
            }
        }

        // PUT
        [HttpPost]
        [ActionName("Upsert")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upsert(ProductViewModel viewModel, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            string wwwRootPath = _webHostEnvironment.WebRootPath;

            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();

                var upload = Path.Combine(wwwRootPath, @"images\products");
                var extension = Path.GetExtension(file.FileName);

                if (viewModel.Product.ImageUrl != null)
                {
                    var oldImage = Path.Combine(wwwRootPath, viewModel.Product.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImage))
                    {
                        System.IO.File.Delete(oldImage);
                    }
                }

                using (var fileStream = new FileStream(
                    Path.Combine(upload, fileName + extension),
                    FileMode.Append,
                    FileAccess.Write,
                    FileShare.None,
                    bufferSize: 4096,
                    useAsync: true))
                {
                    await file.CopyToAsync(fileStream);
                }

                viewModel.Product.ImageUrl = @"\images\products\" + fileName + extension;
            }

            string notification = null;

            if (viewModel.Product.Id == Guid.Empty)
            {
                await _unitOfWork.ProductRepository.AddEntityAsync(viewModel.Product);

                notification = Notifications.ProductCreateSuccess;
            }
            else
            {
                await _unitOfWork.ProductRepository.UpdateEntityAsync(viewModel.Product);

                notification = Notifications.ProductUpdateSuccess;
            }

            await _unitOfWork.SaveAsync();

            _toastNotification.Success(notification);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return NotFound();
            }

            var category = await _unitOfWork.ProductRepository.GetEntityAsync(c => c.Id == id);

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
            var deletedItem = await _unitOfWork.ProductRepository.RemoveEntityAsync(id);

            if (deletedItem == null)
            {
                _toastNotification.Error(Errors.CoverTypeDoesNotExist);

                return View(nameof(Index));
            }

            await _unitOfWork.SaveAsync();

            _toastNotification.Success(Notifications.CoverTypeDeleteSuccess);

            return RedirectToAction(nameof(Index));
        }

        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productList = await _unitOfWork.ProductRepository.GetAllEntitiesAsync(
                includeProps: "Category,CoverType");

            return Json(new { data = productList });
        }
        #endregion
    }
}
