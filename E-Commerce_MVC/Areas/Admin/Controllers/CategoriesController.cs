using BLL.DTOs.Admin;
using BLL.Services.AdminCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly IAdminCategoryService _categoryService;


        public CategoriesController(IAdminCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: /Admin/Categories
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetAllCategoriesAsync(cancellationToken);
            return View(categories);
        }



        // GET: /Admin/Categories/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CategoryCreateDto());
        }

        // POST: /Admin/Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateDto categoryDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryDto);
            }
            try
            {
                await _categoryService.CreateCategoryAsync(categoryDto, cancellationToken);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("Name", ex.Message);
                return View(categoryDto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(categoryDto);
            }
        }

        // GET: /Admin/Categories/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(string id, CancellationToken cancellationToken)
        {
            var categoryDto = await _categoryService.GetCategoryByIdAsync(id, cancellationToken);
            if (categoryDto == null)
            {
                return NotFound();
            }
            return View(categoryDto);
        }

        // POST: /Admin/Categories/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, CategoryAdminDto categoryDto, CancellationToken cancellationToken)
        {
            if (id != categoryDto.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(categoryDto);
            }

            try
            {
                await _categoryService.UpdateCategoryAsync(categoryDto, cancellationToken);
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("Name", ex.Message);
                return View(categoryDto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(categoryDto);
            }
        }

        // GET: /Admin/Categories/Delete/{id}
        [HttpGet]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            var categoryDto = await _categoryService.GetCategoryByIdAsync(id, cancellationToken);
            if (categoryDto == null)
            {
                return NotFound();
            }
            return View(categoryDto);
        }

        // POST: /Admin/Categories/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id, CancellationToken cancellationToken)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id, cancellationToken);
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Delete), new { id = id });
            }
        }
    }
}