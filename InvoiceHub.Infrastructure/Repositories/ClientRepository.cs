using InvoiceHub.Domain.Entities;
using InvoiceHub.Domain.Interfaces;
using InvoiceHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InvoiceHub.Infrastructure.Repositories
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<Client?> GetByIdAsync(int id)
        {
            return await _context.Clients
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Client?> GetByDocumentAsync(string document)
        {
            return await _context.Clients
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(c => c.Document == document);
        }

        public async Task<(IEnumerable<Client> Items, int TotalCount)> GetPagedAsync(
            string? search,
            int page,
            int pageSize,
            bool? isActive)
        {
            var query = _context.Clients
                .IgnoreQueryFilters()
                .AsQueryable();

            if (isActive.HasValue)
            {
                query = query.Where(c => c.IsActive == isActive.Value);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c =>
                    c.Name.Contains(search) ||
                    c.Document.Contains(search));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<bool> ChangeStatusAsync(
            int id,
            bool isActive)
        {
            var client = await _context.Clients
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client is null)
            {
                return false;
            }

            client.IsActive = isActive;

            return true;
        }

    }
}