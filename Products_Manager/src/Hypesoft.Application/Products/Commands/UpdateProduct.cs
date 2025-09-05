using FluentValidation;
using Hypesoft.Application.DTOs;
using MediatR;

namespace Hypesoft.Application.Products.Commands
{
    // O comando recebe um DTO
    public record UpdateProductCommand(ProductUpdateDto Dto) : IRequest<ProductReadDto>;

    public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Dto.Id).NotEmpty();
            RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Dto.Price).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Dto.StockQuantity).GreaterThanOrEqualTo(0);
        }
    }
}
