using System.Security.Claims;
using AutoMapper;
using BLL.DTOs.ReviewsDTOs;
using BLL.Services.Review_Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_MVC.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;

        public ReviewsController(IReviewService reviewService , IMapper mapper)
        {
            _reviewService = reviewService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(CreateReviewDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _reviewService.AddReviewAsync(userId, dto);
            return RedirectToAction("Details", "Orders", new { id = dto.OrderId });
        }

        public async Task<IActionResult> AddReviewPartial(CreatePartailReviewViewModel vm)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var VM = _mapper.Map<CreateReviewDto>(vm);
            await _reviewService.AddReviewAsync(userId, VM);
            return RedirectToAction("Details", "Product", new { id = VM.ProductId });
        }

        public async Task<IActionResult> Open(string productId , string orderId)
        {
            var model = new CreateReviewDto
            {
                ProductId = productId,
                OrderId = orderId
            };

            return View("AddReview", model);
        }
    }
}
