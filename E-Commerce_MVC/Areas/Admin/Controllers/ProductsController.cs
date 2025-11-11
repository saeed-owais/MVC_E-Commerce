using BLL.DTOs.Admin;
using BLL.Services;
using BLL.Services.AdminCategory;
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

        public ProductsController(IAdminProductService productService, IAdminCategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        // ميثود مساعدة لملء قائمة الفئات
        private async Task LoadCategoriesDropdown(CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetAllCategoriesAsync(cancellationToken);
            // نحول قائمة الـ DTOs إلى SelectList ليستخدمها الـ View
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
            await LoadCategoriesDropdown(cancellationToken); // ملء الـ Dropdown
            return View();
        }

        // POST: /Admin/Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductAdminDto productDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoriesDropdown(cancellationToken); // ملء الـ Dropdown مرة أخرى
                return View(productDto);
            }

            try
            {
                await _productService.CreateProductAsync(productDto, cancellationToken);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // نعالج الأخطاء القادمة من الـ BLL
                ModelState.AddModelError("", ex.Message);
                await LoadCategoriesDropdown(cancellationToken);
                return View(productDto);
            }
        }

        // GET: /Admin/Products/Edit/{id}
        public async Task<IActionResult> Edit(string id, CancellationToken cancellationToken)
        {
            var product = await _productService.GetProductByIdAsync(id, cancellationToken);
            if (product == null)
            {
                return NotFound();
            }
            await LoadCategoriesDropdown(cancellationToken);
            return View(product);
        }

        // POST: /Admin/Products/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ProductAdminDto productDto, CancellationToken cancellationToken)
        {
            if (id != productDto.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                await LoadCategoriesDropdown(cancellationToken);
                return View(productDto);
            }

            try
            {
                await _productService.UpdateProductAsync(productDto, cancellationToken);
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
                return View(productDto);
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
