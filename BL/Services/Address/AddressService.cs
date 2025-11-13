using AutoMapper;
using BLL.DTOs.Address;
using DAL.Interfaces;


namespace BLL.Services.Address
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public AddressService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<List<AddressDto>> GetAllAsync(String UserId)
        {
            var addresses =  _uow.Addresses.GetQueryable().Where(a => a.UserId== UserId);
            return _mapper.Map<List<AddressDto>>(addresses);
        }

        public async Task<AddressDto> GetByIdAsync(string id)
        {
            var address = await _uow.Addresses.GetByIdAsync(id);
            return _mapper.Map<AddressDto>(address);

        }

        public async Task AddAsync(AddressDto addressDto)
        {
            addressDto.Id= Guid.NewGuid().ToString();
            var address = _mapper.Map<DA.Models.Address>(addressDto);
            await _uow.Addresses.AddAsync(address);
            await _uow.CompleteAsync();
        }

        public async Task UpdateAsync(AddressDto addressDto)
        {
            var address = _mapper.Map<DA.Models.Address>(addressDto);
            _uow.Addresses.Update(address);
            await _uow.CompleteAsync();
        }

        public async Task DeleteAsync(String Id)
        {
            var address = await _uow.Addresses.GetByIdAsync(Id);
            if (address == null) return;

            _uow.Addresses.Remove(address);
            await _uow.CompleteAsync();
        }
    }
}
