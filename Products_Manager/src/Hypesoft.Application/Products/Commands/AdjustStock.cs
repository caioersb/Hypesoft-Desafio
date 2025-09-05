using FluentValidation;
using Hypesoft.Domain.Repositories;
using MediatR;

namespace Hypesoft.Application.Products.Commands
{
    public record AdjustStockCommand(string ProductId, int Delta) : IRequest<int>; // retorna novo estoque

    public class AdjustStockValidator : AbstractValidator<AdjustStockCommand>
    {
        public AdjustStockValidator()
        {
            RuleFor(x => x.Delta).NotEqual(0);
        }
    }
}
