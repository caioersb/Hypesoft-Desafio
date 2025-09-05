using Hypesoft.Application.DTOs;
using Hypesoft.Domain.Repositories;
using MediatR;

namespace Hypesoft.Application.Categories.Queries;

public record ListCategoriesQuery() : IRequest<IReadOnlyList<CategoryReadDto>>;

public class ListCategoriesHandler : IRequestHandler<ListCategoriesQuery, IReadOnlyList<CategoryReadDto>>
{
    private readonly ICategoryRepository _repo;

    public ListCategoriesHandler(ICategoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<IReadOnlyList<CategoryReadDto>> Handle(ListCategoriesQuery request, CancellationToken ct)
    {
        var categories = await _repo.ListAsync(ct);
        return categories.Select(c => new CategoryReadDto(c.Id, c.Name, c.Description)).ToList();
    }
}
