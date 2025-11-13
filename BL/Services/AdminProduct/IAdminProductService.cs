using BLL.DTOs.Admin;

namespace BLL.Services
{
    public interface IAdminProductService
    {
        Task<IEnumerable<ProductAdminDto>> GetAllProductsAsync(CancellationToken cancellationToken = default);

        Task<ProductAdminDto?> GetProductByIdAsync(string id, CancellationToken cancellationToken = default);

        Task<ProductAdminDto> CreateProductAsync(CreateProductDto productDto, CancellationToken cancellationToken = default);

        Task UpdateProductAsync(UpdateProductDto productDto, CancellationToken cancellationToken = default);

        Task DeleteProductAsync(string id, CancellationToken cancellationToken = default);
    }
}
