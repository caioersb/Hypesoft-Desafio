using Hypesoft.Application.DTOs;
using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;
using MediatR;
using AutoMapper;

namespace Hypesoft.Application.Products.Commands
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, ProductReadDto>
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public UpdateProductHandler(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ProductReadDto> Handle(UpdateProductCommand request, CancellationToken ct)
        {
            var product = await _repo.GetByIdAsync(request.Dto.Id, ct)
                          ?? throw new KeyNotFoundException("Product not found");

            product.Name = request.Dto.Name;
            product.Description = request.Dto.Description;
            product.Price = request.Dto.Price;
            product.StockQuantity = request.Dto.StockQuantity;
            product.CategoryId = request.Dto.CategoryId;
            product.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(product, ct);
            return _mapper.Map<ProductReadDto>(product);
        }
    }
}
