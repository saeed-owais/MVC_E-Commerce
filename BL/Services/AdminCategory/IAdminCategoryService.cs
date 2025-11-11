using BLL.DTOs.Admin;

namespace BLL.Services.AdminCategory
{
    public interface IAdminCategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken = default);
    }
}
