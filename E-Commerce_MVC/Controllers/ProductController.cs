using BLL.Services.Category;
using BLL.Services.Product;
using E_Commerce_MVC.Models.Product;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index(string? categoryId)
        {
            // Get all categories for filter
            var categories = await _categoryService.GetAllAsync();

            // Get products (filter by category if provided)
            var products = !string.IsNullOrEmpty(categoryId)
                ? await _productService.GetByCategoryAsync(categoryId)
                : await _productService.GetAllAsync();

            var viewModel = new ProductListViewModel
            {
                Categories = categories,
                Products = products,
                SelectedCategoryId = categoryId
            };

            return View(viewModel);
        }

        // 🟢 GET: /Products/Details/{id}
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Product ID is required.");

            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            var viewModel = new ProductDetailsViewModel
            {
                Product = product,
                Quantity = 1
            };

            return View(viewModel);
        }
    }
}
