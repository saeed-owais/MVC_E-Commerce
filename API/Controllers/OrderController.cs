using API.Response;
using API.ViewModel.Order;
using BLL.DTOs.Order;
using BLL.Services.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // لو عندك JWT
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("create")]
        public async Task<ApiResponse<OrderViewModel>> CreateOrder(CreateOrderViewModel orderVM)
        {
            var dto = new CreateOrderDto
            {
                UserId = orderVM.UserId,
                AddressId = orderVM.AddressId,
                PaymentMethod = orderVM.PaymentMethod
            };

            var result = await _orderService.CreateOrderFromCartAsync(dto);

            if (!result.Success)
                return ResponseHelper.Fail<OrderViewModel>(result.Message);

            var vm = new OrderViewModel
            {
                Id = result.Order.Id,
                UserId = result.Order.UserId,
                OrderDate = result.Order.OrderDate,
                Status = result.Order.Status,
                TotalAmount = result.Order.TotalAmount,
                Items = result.Order.Items,
                PaymentMethod = result.Order.PaymentMethod,
            };

            return ResponseHelper.Success(vm, result.Message);
        }
    }
}
