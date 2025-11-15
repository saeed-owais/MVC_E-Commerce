using API.Response;
using API.ViewModel.Address;
using AutoMapper;
using BLL.DTOs.Address;
using BLL.Services.Address;
using DA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize] // لو عندك JWT Authentication
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly IMapper _mapper;

        public AddressController(IAddressService addressService, IMapper mapper)
        {
            _addressService = addressService;
            _mapper = mapper;
        }

        [HttpGet("user/{userId}")]
        public async Task<ApiResponse<IEnumerable<AllAddressViewModel>>> GetAllByUser(string userId)
        {
            var addresses = await _addressService.GetAllAsync(userId);

            var addressViewModels = addresses.Select(address =>
                new AllAddressViewModel
                {
                    Id = address.Id,
                    UserId = address.UserId,
                    Street = address.Street,
                    City = address.City,
                    PostalCode = address.PostalCode,
                    Country = address.Country
                }
            );

            return ResponseHelper.Success(addressViewModels);
        }


        [HttpGet("{id}")]
        public async Task<ApiResponse<AddressViewModel>> GetById(string id)
        {
            var address = await _addressService.GetByIdAsync(id);

            if (address == null)
                return ResponseHelper.Fail<AddressViewModel>("Address not found");

            var vm = new AddressViewModel
            {
                UserId = address.UserId,
                Street = address.Street,
                City = address.City,
                PostalCode = address.PostalCode,
                Country = address.Country
            };

            return ResponseHelper.Success(vm);
        }


        [HttpPost]
        public async Task<ApiResponse<AddressViewModel>> Create(CreateAddressViewModel vm)
        {
            var dto = new AddressDto
            {
                UserId = vm.UserId,
                Street = vm.Street,
                City = vm.City,
                PostalCode = vm.PostalCode,
                Country = vm.Country
            };

            await _addressService.AddAsync(dto);

            var createdAddress = new AddressViewModel
            {
                UserId = vm.UserId,
                Street = vm.Street,
                City = vm.City,
                PostalCode = vm.PostalCode,
                Country = vm.Country
            };

            return ResponseHelper.Success(createdAddress, "Address created successfully");
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<string>> Update(string id, CreateAddressViewModel vm)
        {
            var dto = new AddressDto
            {
                Id = id,
                UserId = vm.UserId,
                Street = vm.Street,
                City = vm.City,
                PostalCode = vm.PostalCode,
                Country = vm.Country
            };
            await _addressService.UpdateAsync(dto);

            return ResponseHelper.Success("Updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<string>> Delete(string id)
        {
            await _addressService.DeleteAsync(id);
            return ResponseHelper.Success("Deleted successfully");
        }
    }
}
