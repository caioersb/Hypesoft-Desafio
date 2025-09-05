using Hypesoft.Application.DTOs;
using Hypesoft.Domain.Repositories;
using MediatR;

namespace Hypesoft.Application.Categories.Queries;

public record GetCategoryByIdQuery(string Id) : IRequest<CategoryReadDto?>;

public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, CategoryReadDto?>
{
    private readonly ICategoryRepository _repo;

    public GetCategoryByIdHandler(ICategoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<CategoryReadDto?> Handle(GetCategoryByIdQuery request, CancellationToken ct)
    {
        var category = await _repo.GetByIdAsync(request.Id, ct);
        if (category == null) return null;

        return new CategoryReadDto(category.Id, category.Name, category.Description);
    }
}
