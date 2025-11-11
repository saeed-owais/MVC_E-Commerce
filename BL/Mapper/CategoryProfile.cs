using AutoMapper;
using BLL.DTOs.Admin;
using DA.Models;

namespace BLL.Mapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>();
        }
    }
}
