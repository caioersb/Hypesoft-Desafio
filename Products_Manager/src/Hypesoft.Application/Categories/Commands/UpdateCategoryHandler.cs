using Hypesoft.Application.Categories.Commands;
using Hypesoft.Domain.Repositories;
using MediatR;

namespace Hypesoft.Application.Categories.Handlers
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Unit>
    {
        private readonly ICategoryRepository _repo;

        public UpdateCategoryHandler(ICategoryRepository repo) => _repo = repo;

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken ct)
        {
            var category = await _repo.GetByIdAsync(request.Dto.Id, ct)
                ?? throw new KeyNotFoundException("Category not found");

            category.Name = request.Dto.Name;
            category.Description = request.Dto.Description;

            await _repo.UpdateAsync(category, ct);
            return Unit.Value;
        }
    }
}
