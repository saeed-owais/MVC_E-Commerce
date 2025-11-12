using AutoMapper;
using AutoMapper.QueryableExtensions;
using BLL.DTOs.Product;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // 🟢 Get all products (uses AutoMapper projection)
        public async Task<IEnumerable<ProductDTO>> GetAllAsync()
        {
            return await _unitOfWork.Products
                .GetQueryable()
                .ProjectTo<ProductDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        // 🟢 Get product by ID
        public async Task<ProductDTO?> GetByIdAsync(string id)
        {
            var product = await _unitOfWork.Products
                .GetQueryable()
                .Where(p => p.Id == id)
                .ProjectTo<ProductDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return product;
        }

        // 🟢 Get products by category
        public async Task<IEnumerable<ProductDTO>> GetByCategoryAsync(string categoryId)
        {
            return await _unitOfWork.Products
                .GetQueryable()
                .Where(p => p.CategoryId == categoryId)
                .ProjectTo<ProductDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        // 🟢 Add new product
        public async Task<ProductDTO> AddAsync(ProductDTO dto)
        {
            var entity = _mapper.Map<DA.Models.Product>(dto);
            await _unitOfWork.Products.AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ProductDTO>(entity);
        }

        // 🟢 Update product
        public async Task UpdateAsync(ProductDTO dto)
        {
            var existing = await _unitOfWork.Products.GetByIdAsync(dto.Id);
            if (existing == null)
                throw new KeyNotFoundException($"Product with ID {dto.Id} not found");

            _mapper.Map(dto, existing);
            _unitOfWork.Products.Update(existing);
            await _unitOfWork.CompleteAsync();
        }

        // 🟢 Delete product
        public async Task DeleteAsync(string id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found");

            _unitOfWork.Products.Remove(product);
            await _unitOfWork.CompleteAsync();
        }
    }
}
