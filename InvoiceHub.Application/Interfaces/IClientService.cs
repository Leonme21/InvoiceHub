using InvoiceHub.Application.Dtos.Client;
using InvoiceHub.Application.Dtos.Common;
using InvoiceHub.Application.Dtos.Invoice;

namespace InvoiceHub.Application.Interfaces
{
    public interface IClientService
    {
        Task<ClientDetailResponseDto> CreateAsync(CreateClientDto dto);

        Task<ClientDetailResponseDto> UpdateAsync(
            int id,
            UpdateClientDto dto);

        Task DeleteAsync(int id);

        Task ChangeStatusAsync(
            int id,
            bool isActive);

        Task<IEnumerable<ClientListResponseDto>> GetAllAsync();

        Task<ClientDetailResponseDto?> GetByDocumentAsync(
            string document);

        Task<ClientDetailResponseDto?> GetByIdAsync(
            int id);

        Task<PagedResult<ClientListResponseDto>> GetPagedAsync(
            string? search,
            int page,
            int pageSize,
            bool? isActive);

    }
}