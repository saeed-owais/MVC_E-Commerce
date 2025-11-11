using AutoMapper;
using BLL.DTOs.Admin;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.AdminCategory
{
    public class AdminCategoryService : IAdminCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AdminCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken = default)
        {
            // نستخدم نفس تقنية IQueryable + ProjectTo
            var query = _unitOfWork.Categories.GetQueryable();
            return await _mapper.ProjectTo<CategoryDto>(query)
                                .ToListAsync(cancellationToken);
        }
    }
}
