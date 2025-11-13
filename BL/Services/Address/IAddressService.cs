using BLL.DTOs.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Address
{
    public interface IAddressService
    {
        public Task<List<AddressDto>> GetAllAsync(String UserId);
        public Task<AddressDto> GetByIdAsync(string id);

        public Task AddAsync(AddressDto addressDto);
        public Task UpdateAsync(AddressDto addressDto);
        public Task DeleteAsync(String Id);

    }
}
