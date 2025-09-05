using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;
using Hypesoft.Infrastructure.Data;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Hypesoft.Infrastructure.Repositories;

public class MongoProductRepository : IProductRepository
{
    private readonly IMongoCollection<Product> _collection;

    public MongoProductRepository(MongoDbContext context)
    {
        _collection = context.GetCollection<Product>("Products");
    }

    public async Task AddAsync(Product product, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(product.Id))
            product.Id = ObjectId.GenerateNewId().ToString();

        await _collection.InsertOneAsync(product, null, ct);
    }

    public async Task DeleteAsync(string id, CancellationToken ct) =>
        await _collection.DeleteOneAsync(p => p.Id == id, ct);

    public async Task<Product?> GetByIdAsync(string id, CancellationToken ct) =>
        await _collection.Find(p => p.Id == id).FirstOrDefaultAsync(ct);

    public async Task<IReadOnlyList<Product>> GetLowStockAsync(int threshold, CancellationToken ct) =>
        await _collection.Find(p => p.StockQuantity < threshold).ToListAsync(ct);

    public async Task<(IReadOnlyList<Product> Items, long Total)> SearchAsync(
        string? name, string? categoryId, int page, int pageSize, CancellationToken ct)
    {
        var filter = Builders<Product>.Filter.Empty;

        if (!string.IsNullOrEmpty(name))
            filter &= Builders<Product>.Filter.Regex(p => p.Name, new BsonRegularExpression(name, "i"));

        if (!string.IsNullOrEmpty(categoryId))
            filter &= Builders<Product>.Filter.Eq(p => p.CategoryId, categoryId);

        var total = await _collection.CountDocumentsAsync(filter, cancellationToken: ct);
        var items = await _collection.Find(filter)
                                     .Skip((page - 1) * pageSize)
                                     .Limit(pageSize)
                                     .ToListAsync(ct);
        return (items, total);
    }

    public async Task UpdateAsync(Product product, CancellationToken ct) =>
        await _collection.ReplaceOneAsync(p => p.Id == product.Id, product, new ReplaceOptions { IsUpsert = false }, ct);

    public async Task<decimal> GetTotalInventoryValueAsync(CancellationToken ct)
    {
        var products = await _collection.Find(Builders<Product>.Filter.Empty).ToListAsync(ct);
        return products.Sum(p => p.Price * p.StockQuantity);
    }

    public async Task<long> CountAsync(CancellationToken ct) =>
        await _collection.CountDocumentsAsync(Builders<Product>.Filter.Empty, cancellationToken: ct);

    public async Task<IReadOnlyList<Product>> ListAsync(CancellationToken ct) =>
        await _collection.Find(Builders<Product>.Filter.Empty).ToListAsync(ct);
}
