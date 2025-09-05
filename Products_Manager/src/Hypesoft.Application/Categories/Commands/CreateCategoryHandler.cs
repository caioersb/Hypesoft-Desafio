using Hypesoft.Application.Categories.Commands;
using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;
using MediatR;

namespace Hypesoft.Application.Categories.Handlers
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, string>
    {
        private readonly ICategoryRepository _repo;

        public CreateCategoryHandler(ICategoryRepository repo) => _repo = repo;

        public async Task<string> Handle(CreateCategoryCommand request, CancellationToken ct)
        {
            var c = new Category
            {
                Name = request.Dto.Name,
                Description = request.Dto.Description
            };

            await _repo.AddAsync(c, ct);
            return c.Id;
        }
    }
}
