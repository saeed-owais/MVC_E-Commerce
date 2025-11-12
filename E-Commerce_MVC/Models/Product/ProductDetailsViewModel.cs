using BLL.DTOs.Product;

namespace E_Commerce_MVC.Models.Product
{
    public class ProductDetailsViewModel
    {
        public ProductDTO Product { get; set; } = new ProductDTO();
        //  public bool IsInCart { get; set; } = false; // future enhancement if you track cart
        public int Quantity { get; set; } = 1;
    }
}
