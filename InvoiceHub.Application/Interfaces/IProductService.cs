using InvoiceHub.Application.Dtos.Common;
using InvoiceHub.Application.Dtos.Product;

namespace InvoiceHub.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductDetailResponseDto> CreateAsync(
            CreateProductDto dto);

        Task<ProductDetailResponseDto> UpdateAsync(
            int id,
            UpdateProductDto dto);

        Task DeleteAsync(int id);

        Task ChangeStatusAsync(
            int id,
            bool isActive);

        Task<IEnumerable<ProductListResponseDto>> GetAllAsync();

        Task<ProductDetailResponseDto?> GetByIdAsync(int id);

        Task<ProductDetailResponseDto?> GetByCodeAsync(string code);

        Task<PagedResult<ProductListResponseDto>> GetPagedAsync(
            string? searchTerm,
            int page,
            int pageSize,
            bool? isActive);
    }
}