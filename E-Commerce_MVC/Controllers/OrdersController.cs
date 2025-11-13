
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Services.Order_Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    [Authorize]

    public class OrdersController : Controller
    {
        private readonly IAOrderService _orderService;
        private readonly IMapper _mapper;
     
        public OrdersController(IAOrderService orderService , IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _orderService.GetOrderHistoryAsync(userId);
            return View(orders);
        }

        public async Task<IActionResult> Details(string id)
        {
            ViewBag.OrderId = id;
            var order = await _orderService.GetOrderDetailsAsync(id);
            if (order == null)
                return NotFound();

            return View(order);
        }
    }
}
