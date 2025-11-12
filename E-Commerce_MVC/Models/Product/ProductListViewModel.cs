using BLL.DTOs.Category;
using BLL.DTOs.Product;

namespace E_Commerce_MVC.Models.Product
{
    public class ProductListViewModel
    {
        public IEnumerable<ProductDTO> Products { get; set; } = new List<ProductDTO>();
        public IEnumerable<CategoryDTO> Categories { get; set; } = new List<CategoryDTO>();
        public string? SelectedCategoryId { get; set; }
    }
}
