using System.Security.Claims;
using API.Response;
using AutoMapper;
using BLL.DTOs.ReviewsDTOs;
using BLL.Services.Review_Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;

        public ReviewsController(IReviewService reviewService, IMapper mapper)
        {
            _reviewService = reviewService;
            _mapper = mapper;
        }

        [HttpPost("add")]
        public async Task<ApiResponse<object>> AddReview(CreateReviewDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return ResponseHelper.Fail<object>("UnAuthorized");
            }
            await _reviewService.AddReviewAsync(userId, dto);
            return ResponseHelper.Success<object>("Added Successfully");
        }
        [HttpPost("add-partial")]
        public async Task<ApiResponse<object>> AddReviewPartial(CreatePartailReviewViewModel vm)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return ResponseHelper.Fail<object>("UnAuthorized");
            }

            var VM = _mapper.Map<CreateReviewDto>(vm);
            await _reviewService.AddReviewAsync(userId, VM);
            return ResponseHelper.Success<object>("Added Successfully");
        }
        [HttpGet]
        public async Task<ApiResponse<object>>Open([FromQuery] string productId, [FromQuery] string orderId) //Helper For Adding Reviews from page Orders
        {
            if(productId == null || orderId == null)
            {
                return ResponseHelper.Fail<object>("ProductID or OrderID is Unavailable");
            }
            var model = new CreateReviewDto
            {
                ProductId = productId,
                OrderId = orderId
            };


            return ResponseHelper.Success<object>(model);
        }
    }
}
