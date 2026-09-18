using InvoiceHub.Application.Dtos.Common;
using InvoiceHub.Application.Dtos.Product;
using InvoiceHub.Application.Interfaces;
using InvoiceHub.Application.Mappings;
using InvoiceHub.Domain.Entities;
using InvoiceHub.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceHub.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductDetailResponseDto> CreateAsync(CreateProductDto dto)
        {
            var existProduct = await _productRepository.GetByCodeAsync(dto.Code);
            if (existProduct != null) throw new ArgumentException("El código ya se encuentra registrado en el sistema.");
            var product = dto.ToEntity();
            _productRepository.Add(product);
            await _unitOfWork.SaveChangesAsync();
            return product.ToDetailDto();
        }

        public async Task DeleteAsync(int id)
        {
            await ChangeStatusAsync(id, false);
        }

        public async Task<IEnumerable<ProductListResponseDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(x => x.ToListDto());
        }

        public async Task<ProductDetailResponseDto?> GetByCodeAsync(string code)
        {
            var product = await _productRepository.GetByCodeAsync(code);
            return product?.ToDetailDto();
        }

        public async Task<ProductDetailResponseDto?> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product?.ToDetailDto();
        }

        public async Task<ProductDetailResponseDto> UpdateAsync(int id, UpdateProductDto dto)
        {
            var existProduct = await _productRepository.GetByIdAsync(id);
            if (existProduct is null) throw new ArgumentException("El producto no existe");
            dto.UpdateEntity(existProduct);
            _productRepository.Update(existProduct);
            await _unitOfWork.SaveChangesAsync();
            return existProduct.ToDetailDto();
        }

        public async Task<PagedResult<ProductListResponseDto>> GetPagedAsync(
            string? searchTerm,
            int page,
            int pageSize,
            bool? isActive)
        {
            var (items, totalCount) =
                await _productRepository.GetPagedAsync(
                    searchTerm,
                    page,
                    pageSize,
                    isActive);

            return new PagedResult<ProductListResponseDto>
            {
                Items = items
                    .Select(p => p.ToListDto())
                    .ToList(),

                TotalCount = totalCount,
                CurrentPage = page,
                PageSize = pageSize
            };
        }


        public async Task ChangeStatusAsync(
            int id,
            bool isActive)
        {
            var updated =
                await _productRepository.ChangeStatusAsync(
                    id,
                    isActive);

            if (!updated)
            {
                throw new KeyNotFoundException(
                    "El producto no existe.");
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
