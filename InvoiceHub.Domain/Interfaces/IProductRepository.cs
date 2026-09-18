using InvoiceHub.Domain.Entities;

namespace InvoiceHub.Domain.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product?> GetByCodeAsync(string code);

        Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(
            string? searchTerm,
            int page,
            int pageSize,
            bool? isActive);

        Task<bool> ChangeStatusAsync(
            int id,
            bool isActive);
    }
}