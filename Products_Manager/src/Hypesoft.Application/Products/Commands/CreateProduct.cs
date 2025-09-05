using FluentValidation;
using Hypesoft.Application.DTOs;
using MediatR;

namespace Hypesoft.Application.Products.Commands
{
    public record CreateProductCommand(ProductCreateDto Dto) : IRequest<ProductReadDto>;

    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Dto.Price).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Dto.CategoryId).NotEmpty();
            RuleFor(x => x.Dto.StockQuantity).GreaterThanOrEqualTo(0);
        }
    }
}
