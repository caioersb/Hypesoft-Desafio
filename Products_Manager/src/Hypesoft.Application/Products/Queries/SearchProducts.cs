using AutoMapper;
using Hypesoft.Application.DTOs;
using Hypesoft.Domain.Repositories;
using MediatR;

public record SearchProductsQuery(string? Name, string? CategoryId, int Page = 1, int PageSize = 20)
    : IRequest<(IReadOnlyList<ProductReadDto> Items, long Total)>;

public class SearchProductsHandler : IRequestHandler<SearchProductsQuery, (IReadOnlyList<ProductReadDto>, long)>
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;

    public SearchProductsHandler(IProductRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<(IReadOnlyList<ProductReadDto>, long)> Handle(SearchProductsQuery q, CancellationToken ct)
    {
        var (products, total) = await _repo.SearchAsync(q.Name, q.CategoryId, q.Page, q.PageSize, ct);

        // converte os entities para DTOs
        var items = _mapper.Map<IReadOnlyList<ProductReadDto>>(products);

        return (items, total);
    }
}
