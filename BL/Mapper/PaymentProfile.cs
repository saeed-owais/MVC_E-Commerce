using AutoMapper;
using BLL.DTOs.Payment;
using DA.Models;

namespace BLL.Mapper
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<Payment, PaymentRequestDto>().ReverseMap();

            CreateMap<Payment, PaymentResponseDto>().ReverseMap();
        }
    }
}
