using Hypesoft.Application.Products.Commands;
using Hypesoft.Domain.Repositories;
using MediatR;

namespace Hypesoft.Application.Products.Handlers
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, Unit>
    {
        private readonly IProductRepository _repo;

        public DeleteProductHandler(IProductRepository repo) => _repo = repo;

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken ct)
        {
            await _repo.DeleteAsync(request.Id, ct);
            return Unit.Value;
        }
    }
}
