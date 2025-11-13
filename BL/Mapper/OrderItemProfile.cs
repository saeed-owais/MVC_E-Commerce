using AutoMapper;
using BLL.DTOs.OrderItem;
using DA.Models;

namespace BLL.Mapper
{
    public class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {
            CreateMap<OrderItem, OrderItemDto>()
            .ForMember(d => d.ProductName, opt => opt.MapFrom(src => src.Product.Name));
        }
    }
}
