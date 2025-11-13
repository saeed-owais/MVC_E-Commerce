using AutoMapper;
using BLL.DTOs.Order;
using DA.Models;


namespace BLL.Mapper
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
            .ForMember(d => d.Items, opt => opt.MapFrom(src => src.OrderItems))
            .ForMember(d => d.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(d => d.PaymentMethod, opt => opt.MapFrom(src => src.Payment.PaymentMethod.ToString()))
            .ForMember(d => d.PaymentSuccess, opt => opt.MapFrom(src => src.Payment.IsSuccessful))
            .ForMember(d => d.TransactionId, opt => opt.MapFrom(src => src.Payment.TransactionId));



        }
    }
    
    
}
