using AutoMapper;
using AutoMapper.QueryableExtensions;
using BLL.DTOs.Category;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
        {
            return await _unitOfWork.Categories
                .GetQueryable()
                .ProjectTo<CategoryDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
