using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;
using Hypesoft.Infrastructure.Data;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Hypesoft.Infrastructure.Repositories;

public class MongoCategoryRepository : ICategoryRepository
{
    private readonly IMongoCollection<Category> _collection;

    public MongoCategoryRepository(MongoDbContext context)
    {
        _collection = context.GetCollection<Category>("Categories");
    }

    public async Task AddAsync(Category category, CancellationToken ct)
    {
        // Garante que o Id não seja nulo antes de inserir
        if (string.IsNullOrEmpty(category.Id))
        {
            category.Id = ObjectId.GenerateNewId().ToString();
        }

        await _collection.InsertOneAsync(category, null, ct);
    }

    public async Task DeleteAsync(string id, CancellationToken ct) =>
        await _collection.DeleteOneAsync(c => c.Id == id, ct);

    public async Task<Category?> GetByIdAsync(string id, CancellationToken ct) =>
        await _collection.Find(c => c.Id == id).FirstOrDefaultAsync(ct);

    public async Task<IReadOnlyList<Category>> ListAsync(CancellationToken ct) =>
        await _collection.Find(Builders<Category>.Filter.Empty).ToListAsync(ct);

    public async Task UpdateAsync(Category category, CancellationToken ct) =>
        await _collection.ReplaceOneAsync(c => c.Id == category.Id, category, new ReplaceOptions { IsUpsert = false }, ct);
}
