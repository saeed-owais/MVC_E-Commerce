using BLL.DTOs.Address;
using BLL.Services.Address;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce_MVC.Controllers
{
    public class AddressController : Controller
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public IActionResult AddAddress()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress(AddressDto addressDto)
        {
            addressDto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!ModelState.IsValid)
                return View(addressDto);

            await _addressService.AddAsync(addressDto);
            return RedirectToAction("Index", "Checkout");
        }
        public async Task<IActionResult> EditAddress(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index", "Checkout");

            var address = await _addressService.GetByIdAsync(id);

            if (address == null)
            {
                TempData["Error"] = "Address not found.";
                return RedirectToAction("Index", "Checkout");
            }

            return View(address);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAddress(AddressDto addressDto)
        {
            addressDto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!ModelState.IsValid)
                return View(addressDto);

            await _addressService.UpdateAsync(addressDto);

            TempData["Success"] = "Address updated successfully!";
            return RedirectToAction("Index", "Checkout");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAddress(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["Error"] = "Please select a valid address.";
                return RedirectToAction("Index", "Checkout");
            }

            await _addressService.DeleteAsync(id);

            TempData["Success"] = "Address deleted successfully!";
            return RedirectToAction("Index", "Checkout");
        }

    }
}
