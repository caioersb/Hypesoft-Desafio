using Hypesoft.Application.DTOs;
using Hypesoft.Application.Products.Commands;
using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;
using MediatR;
using AutoMapper;

namespace Hypesoft.Application.Products.Handlers
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductReadDto>
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public CreateProductHandler(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ProductReadDto> Handle(CreateProductCommand request, CancellationToken ct)
        {
            var product = new Product
            {
                Name = request.Dto.Name,
                Description = request.Dto.Description,
                Price = request.Dto.Price,
                CategoryId = request.Dto.CategoryId,
                StockQuantity = request.Dto.StockQuantity
            };

            await _repo.AddAsync(product, ct);
            return _mapper.Map<ProductReadDto>(product);
        }
    }
}
