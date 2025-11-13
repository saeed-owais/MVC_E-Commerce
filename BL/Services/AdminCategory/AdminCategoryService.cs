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

        public async Task<CategoryCreateDto> CreateCategoryAsync(CategoryCreateDto categoryDto, CancellationToken cancellationToken = default)
        {
            var query = _unitOfWork.Categories
                                    .GetQueryable()
                                    .Where(c => c.Name.ToLower() == categoryDto.Name.ToLower());

            if (await query.AnyAsync(cancellationToken))
            {
                throw new InvalidOperationException($"Category with name '{categoryDto.Name}' already exists.");
            }
            var category = _mapper.Map<DA.Models.Category>(categoryDto);
            await _unitOfWork.Categories.AddAsync(category, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return _mapper.Map<CategoryCreateDto>(category);
        }

        public async Task DeleteCategoryAsync(string id, CancellationToken cancellationToken = default)
        {
            var existingCategory = await _unitOfWork.Categories.GetByIdAsync(id, cancellationToken);
            if (existingCategory == null)
            {
                throw new KeyNotFoundException($"Category with ID {id} not found.");
            }
            _unitOfWork.Categories.Remove(existingCategory);

            await _unitOfWork.CompleteAsync(cancellationToken);
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken = default)
        {
            var query = _unitOfWork.Categories.GetQueryable();
            return await _mapper.ProjectTo<CategoryDto>(query)
                                .ToListAsync(cancellationToken);
        }

        public async Task<CategoryAdminDto?> GetCategoryByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var query = _unitOfWork.Categories.GetQueryable().Where(c => c.Id == id);

            return await _mapper.ProjectTo<CategoryAdminDto>(query)
                                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task UpdateCategoryAsync(CategoryAdminDto categoryDto, CancellationToken cancellationToken = default)
        {
            var existingCategory = await _unitOfWork.Categories.GetByIdAsync(categoryDto.Id, cancellationToken);
            if (existingCategory == null)
            {
                throw new KeyNotFoundException($"Category with ID {categoryDto.Id} not found.");
            }

            var query = _unitOfWork.Categories
                                    .GetQueryable()
                                    .Where(c => c.Name.ToLower() == categoryDto.Name.ToLower()
                 && c.Id != categoryDto.Id);

            if (await query.AnyAsync(cancellationToken))
            {
                throw new InvalidOperationException($"Category with name '{categoryDto.Name}' already exists.");
            }
            _mapper.Map(categoryDto, existingCategory);

            _unitOfWork.Categories.Update(existingCategory);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
