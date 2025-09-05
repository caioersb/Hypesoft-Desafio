using FluentValidation;
using Hypesoft.Application.DTOs; 
using MediatR;

namespace Hypesoft.Application.Categories.Commands
{
    public record CreateCategoryCommand(CategoryCreateDto Dto) : IRequest<string>;

    public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Dto.Description).MaximumLength(500);
        }
    }
}
