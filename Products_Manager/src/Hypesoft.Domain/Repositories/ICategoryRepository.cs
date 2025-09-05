using Hypesoft.Domain.Entities;

namespace Hypesoft.Domain.Repositories;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(string id, CancellationToken ct);
    Task<IReadOnlyList<Category>> ListAsync(CancellationToken ct);
    Task AddAsync(Category category, CancellationToken ct);
    Task UpdateAsync(Category category, CancellationToken ct);
    Task DeleteAsync(string id, CancellationToken ct);
}
