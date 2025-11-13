using BLL.DTOs.Admin;

namespace BLL.Services.AdminCategory
{
    public interface IAdminCategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken = default);
        Task<CategoryAdminDto?> GetCategoryByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<CategoryCreateDto> CreateCategoryAsync(CategoryCreateDto categoryDto, CancellationToken cancellationToken = default);
        Task UpdateCategoryAsync(CategoryAdminDto categoryDto, CancellationToken cancellationToken = default);
        Task DeleteCategoryAsync(string id, CancellationToken cancellationToken = default);
    }
}
