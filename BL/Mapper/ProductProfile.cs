
using AutoMapper;
using BLL.DTOs.Admin;
using DA.Models;
namespace BLL.Mapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductAdminDto>()
                .ForMember(dest => dest.CategoryName,
                           opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<ProductAdminDto, Product>()
                .ForMember(dest => dest.Category, opt => opt.Ignore()); // نتجاهل الـ Category Object

            CreateMap<DA.Models.Product, DTOs.Product.ProductDTO>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty));
        }
    }
}
