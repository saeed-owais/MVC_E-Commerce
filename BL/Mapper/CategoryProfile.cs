using AutoMapper;
using BLL.DTOs.Admin;
using BLL.DTOs.Category;
using DA.Models;

namespace BLL.Mapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryCreateDto, Category>().ReverseMap();
            CreateMap<Category, CategoryAdminDto>().ReverseMap();
        }
    }
}
