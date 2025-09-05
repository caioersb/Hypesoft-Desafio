using Hypesoft.Domain.Entities;

namespace Hypesoft.Domain.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(string id, CancellationToken ct);
    Task<(IReadOnlyList<Product> Items, long Total)> SearchAsync(
        string? name, string? categoryId, int page, int pageSize, CancellationToken ct);
    Task<IReadOnlyList<Product>> GetLowStockAsync(int threshold, CancellationToken ct);
    Task<IReadOnlyList<Product>> ListAsync(CancellationToken ct); // <-- Adicionado
    Task AddAsync(Product product, CancellationToken ct);
    Task UpdateAsync(Product product, CancellationToken ct);
    Task DeleteAsync(string id, CancellationToken ct);
    Task<decimal> GetTotalInventoryValueAsync(CancellationToken ct);
    Task<long> CountAsync(CancellationToken ct);
}
