using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTOs.OrderDTOs;
using BLL.DTOs.OrderItemsDTOs;
using BLL.DTOs.ReviewsDTOs;
using DA.Models;

namespace BLL.Mapper
{
    public class OrderHistoryProfile : Profile
    {
       public OrderHistoryProfile() 
        {



            // 🟢 Orders
            CreateMap<Order, OrderHistoryDto>();
            CreateMap<CreatePartailReviewViewModel, CreateReviewDto>();
            //CreateMap<OrderHistoryViewModel, OrderHistoryDto>();

            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            // 🟢 OrderItems
            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.UnitPrice));

            // 🟢 Reviews
            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));

            CreateMap<CreateReviewDto, Review>();
        }
    }
}
