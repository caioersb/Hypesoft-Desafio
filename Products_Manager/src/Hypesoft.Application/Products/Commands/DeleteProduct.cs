using FluentValidation;
using MediatR;

namespace Hypesoft.Application.Products.Commands
{
    public record DeleteProductCommand(string Id) : IRequest<Unit>;

    public class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
