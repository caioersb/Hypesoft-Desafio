using FluentValidation;
using Hypesoft.Application.DTOs;
using MediatR;

namespace Hypesoft.Application.Categories.Commands
{
    public record UpdateCategoryCommand(CategoryUpdateDto Dto) : IRequest<Unit>;

    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Dto.Id).NotEmpty();
            RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Dto.Description).MaximumLength(500);
        }
    }
}
