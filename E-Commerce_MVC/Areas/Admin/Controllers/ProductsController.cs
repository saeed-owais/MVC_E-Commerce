using BLL.DTOs.Admin;
using BLL.Services;
using BLL.Services.AdminCategory;
using E_Commerce_MVC.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_Commerce_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly IAdminProductService _productService;
        private readonly IAdminCategoryService _categoryService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductsController(IAdminProductService productService, IAdminCategoryService categoryService, IWebHostEnvironment webHostEnvironment)
        {
            _productService = productService;
            _categoryService = categoryService;
            _webHostEnvironment = webHostEnvironment;
        }

        private async Task LoadCategoriesDropdown(CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetAllCategoriesAsync(cancellationToken);

            ViewBag.Categories = new SelectList(categories, "Id", "Name");
        }

        // GET: /Admin/Products
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var products = await _productService.GetAllProductsAsync(cancellationToken);
            return View(products);
        }

        // GET: /Admin/Products/Create
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            await LoadCategoriesDropdown(cancellationToken);
            return View(new CreateProductViewModel());
        }

        // POST: /Admin/Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductViewModel viewModel, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoriesDropdown(cancellationToken);
                return View(viewModel);
            }

            try
            {
                string? imageUrl = null;
                if (viewModel.ImageFile != null)
                {
                    imageUrl = await SaveImageAsync(viewModel.ImageFile, cancellationToken);
                }

                var productDto = new CreateProductDto
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    Price = viewModel.Price,
                    Stock = viewModel.Stock,
                    CategoryId = viewModel.CategoryId,
                    ImageURL = imageUrl
                };

                await _productService.CreateProductAsync(productDto, cancellationToken);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                await LoadCategoriesDropdown(cancellationToken);
                return View(viewModel);
            }
        }

        private void DeleteImage(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return;
            }
            try
            {
                var relativePath = imageUrl.TrimStart('/');
                relativePath = relativePath.Replace('/', Path.DirectorySeparatorChar);
                var physicalPath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);

                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                }
            }
            catch (Exception ex)
            {
                // _logger.LogWarning(ex, "Could not delete old file: {path}", imageUrl);
            }
        }

        private async Task<string?> SaveImageAsync(IFormFile imageFile, CancellationToken cancellationToken)
        {
            if (imageFile.Length == 0) return null;

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string imagePath = Path.Combine(wwwRootPath, "images", "products");

            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            string filePath = Path.Combine(imagePath, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream, cancellationToken);
            }

            return "/images/products/" + fileName;
        }

        // GET: /Admin/Products/Edit/{id}
        public async Task<IActionResult> Edit(string id, CancellationToken cancellationToken)
        {
            var productDto = await _productService.GetProductByIdAsync(id, cancellationToken);
            if (productDto == null)
            {
                return NotFound();
            }
            var viewModel = new EditProductViewModel
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Stock = productDto.Stock,
                CategoryId = productDto.CategoryId, // ◀️ لاحظ: نحتاج CategoryId في DTO القراءة
                ExistingImageUrl = productDto.ImageUrl
            };

            await LoadCategoriesDropdown(cancellationToken);
            return View(viewModel); // ◀️ إرسال VM التعديل
        }

        // POST: /Admin/Products/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditProductViewModel viewModel, CancellationToken cancellationToken)
        {
            if (id != viewModel.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                await LoadCategoriesDropdown(cancellationToken);
                return View(viewModel);
            }

            try
            {
                string? imageUrl = viewModel.ExistingImageUrl;

                if (viewModel.ImageFile != null)
                {
                    imageUrl = await SaveImageAsync(viewModel.ImageFile, cancellationToken);
                    DeleteImage(viewModel.ExistingImageUrl);
                }
                var updateDto = new UpdateProductDto
                {
                    Id = viewModel.Id,
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    Price = viewModel.Price,
                    Stock = viewModel.Stock,
                    CategoryId = viewModel.CategoryId,
                    ImageUrl = imageUrl
                };

                await _productService.UpdateProductAsync(updateDto, cancellationToken);

                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                await LoadCategoriesDropdown(cancellationToken);
                return View(viewModel);
            }
        }

        // GET: /Admin/Products/Delete/{id}
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            var product = await _productService.GetProductByIdAsync(id, cancellationToken);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: /Admin/Products/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id, CancellationToken cancellationToken)
        {
            try
            {
                await _productService.DeleteProductAsync(id, cancellationToken);
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                // يمكنك إضافة رسالة خطأ هنا
                return RedirectToAction(nameof(Delete), new { id = id, error = true });
            }
        }
    }
}
