using InvoiceHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceHub.Domain.Interfaces
{
    public interface IInvoiceRepository : IRepository<Invoice>
    {
        Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber);
        Task<IEnumerable<Invoice>> GetByClientIdAsync(int clientId);
        Task<(IEnumerable<Invoice> Items, int TotalCount)> GetPagedAsync(string? searchTerm, int page, int pageSize);

    }
}
