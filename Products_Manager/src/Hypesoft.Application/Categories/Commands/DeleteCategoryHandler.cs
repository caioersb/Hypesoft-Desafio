using Hypesoft.Application.Categories.Commands;
using Hypesoft.Domain.Repositories;
using MediatR;

namespace Hypesoft.Application.Categories.Handlers
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, Unit>
    {
        private readonly ICategoryRepository _repo;

        public DeleteCategoryHandler(ICategoryRepository repo) => _repo = repo;

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken ct)
        {
            await _repo.DeleteAsync(request.Id, ct);
            return Unit.Value;
        }
    }
}
