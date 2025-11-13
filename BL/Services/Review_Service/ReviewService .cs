
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTOs.ReviewsDTOs;
using DA.Models;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore; 

namespace BLL.Services.Review_Service
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsForProductAsync(string productId)
        {
            var reviews = await _unitOfWork.Reviews
                .GetQueryable()
                .Where(r => r.ProductId == productId && !r.IsDeleted)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }

        public async Task AddReviewAsync(string userId, CreateReviewDto dto)
        {
            var review = _mapper.Map<Review>(dto);
            review.UserId = userId;
            review.CreatedOnUtc = DateTime.UtcNow;
            review.Id = Guid.NewGuid().ToString();

            await _unitOfWork.Reviews.AddAsync(review);
            await _unitOfWork.CompleteAsync();
        }
    }
}

