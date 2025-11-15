using System.Security.Claims;
using API.Response;
using API.ViewModel.CartItem;
using AutoMapper;
using BLL.DTOs.OrderDTOs;
using BLL.Services.Order_Service;
using DA.Models;
using DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IAOrderService _orderService;
        public OrdersController(IAOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _orderService.GetOrderHistoryAsync(userId);
            if (orders == null)
            {
                return NotFound( ResponseHelper.Fail<OrderHistoryDto>("Not Found"));
            }
            return Ok(ResponseHelper.Success(orders));
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Details(string id)
        {
            var order = await _orderService.GetOrderDetailsAsync(id);
            if (order == null)
                return NotFound(ResponseHelper.Fail<OrderDTO>("Not Found"));

            return Ok(ResponseHelper.Success(order));
        }
    }
}
