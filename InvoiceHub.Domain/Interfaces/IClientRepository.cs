using InvoiceHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceHub.Domain.Interfaces
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<Client?> GetByDocumentAsync(string document);
        Task<(IEnumerable<Client> Items, int TotalCount)> GetPagedAsync(
        string? search,
        int page,
        int pageSize,
        bool? isActive);

        Task<bool> ChangeStatusAsync(int id, bool isActive);

    }
}
