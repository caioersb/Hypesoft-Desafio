using AutoMapper;
using Hypesoft.Application.DTOs;
using Hypesoft.Domain.Repositories;
using MediatR;

namespace Hypesoft.Application.Products.Queries
{
    public record ListProductsQuery() : IRequest<IReadOnlyList<ProductReadDto>>;

    public class ListProductsHandler : IRequestHandler<ListProductsQuery, IReadOnlyList<ProductReadDto>>
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public ListProductsHandler(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ProductReadDto>> Handle(ListProductsQuery request, CancellationToken ct)
        {
            var products = await _repo.ListAsync(ct);
            return _mapper.Map<IReadOnlyList<ProductReadDto>>(products);
        }
    }
}
