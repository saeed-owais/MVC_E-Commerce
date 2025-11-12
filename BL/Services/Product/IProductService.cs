using BLL.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Product
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllAsync();
        Task<ProductDTO> GetByIdAsync(string id);
        Task<IEnumerable<ProductDTO>> GetByCategoryAsync(string categoryId);

    }
}
