using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SimpleWebStore.DAL.UnitOfWork;
using SimpleWebStore.Domain.Constants;
using SimpleWebStore.UI.Controllers;
using SimpleWebStore.UI.ViewModels;

namespace SimpleWebStore.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : GeneralController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<CategoryController> _logger;

        public ProductController(IWebHostEnvironment webHostEnvironment,
            IUnitOfWork unitOfWork,
            ILogger<CategoryController> logger,
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
                var url = viewModel.Product.ImageUrl;

                if (url != null)
                {
                    await DeletePhoto(url, _webHostEnvironment);
                }

                viewModel.Product.ImageUrl = await UploadPhoto(url, file, _webHostEnvironment);
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

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedItem = await _unitOfWork.ProductRepository.GetEntityAsync(c => c.Id == id);

            if (deletedItem == null)
            {
                return Json(new { success = false, message = Errors.ProductDeletingError });
            }

            await DeletePhoto(deletedItem.ImageUrl, _webHostEnvironment);

            await _unitOfWork.ProductRepository.RemoveEntityAsync(e => e.Id == id);

            await _unitOfWork.SaveAsync();

            return Json(new { success = true, message = Notifications.ProductDeleteSuccess });
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

        #region Private methods

        private async Task<string> UploadPhoto(string url, IFormFile? file, IWebHostEnvironment environment)
        {
            string fileName = Guid.NewGuid().ToString();

            var upload = Path.Combine(environment.WebRootPath, @"images\products");
            var extension = Path.GetExtension(file.FileName);

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

            url = @"\images\products\" + fileName + extension;

            return url;
        }

        private async Task<bool> DeletePhoto(string url, IWebHostEnvironment environment)
        {
            var oldImage = Path.Combine(environment.WebRootPath, url.TrimStart('\\'));

            if (!System.IO.File.Exists(oldImage))
            {
                return false;
            }

            System.IO.File.Delete(oldImage);

            return true;
        }

        #endregion
    }
}