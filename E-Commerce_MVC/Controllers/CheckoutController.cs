using BLL.DTOs.Order;
using BLL.Services.Address;
using BLL.Services.Cartitem;
using BLL.Services.Order;
using BLL.Services.OrderItem;
using E_Commerce_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class CheckoutController(IOrderService orderService, IAddressService addressService , ICartItemService cartItemService) : Controller
{
 
    public async Task<IActionResult> Index()
    {
        //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = "21b13fbc-7a0d-4355-a6cf-91ef672326d9";


        var vm = new CheckoutViewModel
        {
            
             
            CartItems = await cartItemService.GetAllAsync(),
            Addresses = await addressService.GetAllAsync(userId),
            CreateOrderDto = new CreateOrderDto
            {
                UserId = userId
            }
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Process(CheckoutViewModel model)
    {
        var dto = model.CreateOrderDto;
        var result = await orderService.CreateOrderFromCartAsync(dto);

        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction("Index");
        }

        return RedirectToAction("Success", new { id = result.Order.Id });
    }

    public IActionResult Success(string id)
    {
        ViewBag.OrderId = id;
        return View();
    }
}
