using BLL.DTOs.Address;
using DA.Models;


namespace BLL.Mapper
{
    public class AddressProfile : AutoMapper.Profile
    {
        public AddressProfile() {
            CreateMap<Address, AddressDto>()
                .ForMember(p=> p.UserId, opt => opt.MapFrom(a => a.UserId)).ReverseMap();

            //CreateMap<AddressDto, API.ViewModel.AddressViewModel>().ReverseMap();

        }
    }
}
