using AutoMapper;
using BLL.DTOs.Admin;
using DA.Models;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.AdminProduct
{
    public class AdminProductService : IAdminProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AdminProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductAdminDto>> GetAllProductsAsync(CancellationToken cancellationToken = default)
        {
            var productsQuery = _unitOfWork.Products.GetQueryable();

            return await _mapper.ProjectTo<ProductAdminDto>(productsQuery)
                                .ToListAsync(cancellationToken);
        }

        public async Task<ProductAdminDto?> GetProductByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var productQuery = _unitOfWork.Products.GetQueryable()
                                        .Where(p => p.Id == id);

            return await _mapper.ProjectTo<ProductAdminDto>(productQuery)
                                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<ProductAdminDto> CreateProductAsync(ProductAdminDto productDto, CancellationToken cancellationToken = default)
        {
            var categoryExists = await _unitOfWork.Categories.GetByIdAsync(productDto.CategoryId, cancellationToken);
            if (categoryExists == null)
            {
                throw new InvalidOperationException($"Category with ID {productDto.CategoryId} does not exist.");
            }

            var product = _mapper.Map<DA.Models.Product>(productDto);

            await _unitOfWork.Products.AddAsync(product, cancellationToken);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ProductAdminDto>(product);
        }

        public async Task UpdateProductAsync(ProductAdminDto productDto, CancellationToken cancellationToken = default)
        {
            var existingProduct = await _unitOfWork.Products.GetByIdAsync(productDto.Id, cancellationToken);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with ID {productDto.Id} not found.");
            }

            if (existingProduct.CategoryId != productDto.CategoryId)
            {
                var categoryExists = await _unitOfWork.Categories.GetByIdAsync(productDto.CategoryId, cancellationToken);
                if (categoryExists == null)
                {
                    throw new InvalidOperationException($"Category with ID {productDto.CategoryId} does not exist.");
                }
            }

            _mapper.Map(productDto, existingProduct);

            _unitOfWork.Products.Update(existingProduct);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteProductAsync(string id, CancellationToken cancellationToken = default)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id, cancellationToken);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            }

            _unitOfWork.Products.Remove(product);
            await _unitOfWork.CompleteAsync();
        }
    }
}
