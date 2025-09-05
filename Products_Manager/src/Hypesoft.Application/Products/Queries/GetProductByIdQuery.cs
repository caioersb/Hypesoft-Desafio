using Hypesoft.Application.DTOs;
using Hypesoft.Domain.Repositories;
using MediatR;

namespace Hypesoft.Application.Products.Queries;

public record GetProductByIdQuery(string Id) : IRequest<ProductReadDto?>;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductReadDto?>
{
    private readonly IProductRepository _repo;

    public GetProductByIdHandler(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<ProductReadDto?> Handle(GetProductByIdQuery request, CancellationToken ct)
    {
        var product = await _repo.GetByIdAsync(request.Id, ct);
        if (product == null) return null;

        return new ProductReadDto(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.CategoryId,
            product.StockQuantity
        );
    }
}
