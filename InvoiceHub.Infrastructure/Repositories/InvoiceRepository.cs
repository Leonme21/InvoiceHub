using InvoiceHub.Domain.Entities;
using InvoiceHub.Domain.Interfaces;
using InvoiceHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceHub.Infrastructure.Repositories
{
    public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(ApplicationDbContext context) : base(context)
        {
        }
        public override async Task<IEnumerable<Invoice>> GetAllAsync()
        {
            return await _context.Invoices
                .IgnoreQueryFilters()
                .Include(i => i.Client)
                .OrderByDescending(i => i.Id)
                .ToListAsync();
        }

        public override async Task<Invoice?> GetByIdAsync(int id)
        {
            return await _context.Invoices
                .IgnoreQueryFilters()
                .Include(i => i.Client)
                .Include(i => i.InvoiceDetails)
                    .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber)
        {
            return await _context.Invoices
                .IgnoreQueryFilters()
                .Include(i => i.Client)
                .Include(i => i.InvoiceDetails)
                    .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber);
        }

        public async Task<IEnumerable<Invoice>> GetByClientIdAsync(
            int clientId)
        {
            return await _context.Invoices
                .IgnoreQueryFilters()
                .Include(i => i.Client)
                .Where(i => i.ClientId == clientId)
                .OrderByDescending(i => i.Id)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Invoice> Items, int TotalCount)> GetPagedAsync(
            string? searchTerm,
            int page,
            int pageSize)
        {
            var query = _context.Invoices
                .IgnoreQueryFilters()
                .Include(i => i.Client)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(i =>
                    i.InvoiceNumber.Contains(searchTerm) ||
                    (i.Client != null &&
                     i.Client.Name.Contains(searchTerm)));
            }

            var totalCount =
                await query.CountAsync();

            var items = await query
                .OrderByDescending(i => i.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

    }
}
