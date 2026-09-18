using InvoiceHub.Application.Dtos.Common;
using InvoiceHub.Application.Dtos.Invoice;
using InvoiceHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceHub.Application.Interfaces
{
    public interface IInvoiceService
    {
        Task<InvoiceResponseDto> CreateAsync(InvoiceRequestDto dto);
        Task<IEnumerable<InvoiceListResponseDto>> GetByClientIdAsync(int clientId);
        Task<IEnumerable<InvoiceListResponseDto>> GetAllAsync();
        Task<InvoiceResponseDto?> GetByIdAsync(int id);
        Task<PagedResult<InvoiceListResponseDto>> GetPagedAsync(string? searchTerm, int page, int pageSize);
    }
}
