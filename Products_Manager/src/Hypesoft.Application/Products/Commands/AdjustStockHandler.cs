using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;
using MediatR;

namespace Hypesoft.Application.Products.Commands
{
    public class AdjustStockHandler : IRequestHandler<AdjustStockCommand, int>
    {
        private readonly IProductRepository _repo;

        public AdjustStockHandler(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<int> Handle(AdjustStockCommand request, CancellationToken ct)
        {
            var product = await _repo.GetByIdAsync(request.ProductId, ct)
                         ?? throw new KeyNotFoundException("Product not found");

            var newQty = product.StockQuantity + request.Delta;
            if (newQty < 0)
                throw new InvalidOperationException("Stock cannot be negative");

            product.StockQuantity = newQty;
            product.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(product, ct);
            return newQty;
        }
    }
}
