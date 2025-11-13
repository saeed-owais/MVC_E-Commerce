using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTOs.ReviewsDTOs;

namespace BLL.Services.Review_Service
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDto>> GetReviewsForProductAsync(string productId);
        Task AddReviewAsync(string userId, CreateReviewDto dto);
    }

}
