using InvoiceHub.Domain.Entities;
using InvoiceHub.Domain.Interfaces;
using InvoiceHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InvoiceHub.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<Product?> GetByCodeAsync(string code)
        {
            return await _context.Products
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Code == code);
        }

        public override async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(
            string? search,
            int page,
            int pageSize,
            bool? isActive)
        {
            var query = _context.Products
                .IgnoreQueryFilters()
                .AsQueryable();

            if (isActive.HasValue)
            {
                query = query.Where(
                    p => p.IsActive == isActive.Value);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p =>
                    p.Name.Contains(search) ||
                    p.Code.Contains(search));
            }

            var totalCount =
                await query.CountAsync();

            var items = await query
                .OrderByDescending(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<bool> ChangeStatusAsync(
            int id,
            bool isActive)
        {
            var product = await _context.Products
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product is null)
            {
                return false;
            }

            product.IsActive = isActive;

            return true;
        }
    }
}