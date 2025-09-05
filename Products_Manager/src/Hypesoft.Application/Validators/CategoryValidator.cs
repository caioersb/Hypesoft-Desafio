using FluentValidation;
using Hypesoft.Application.DTOs;

public class CategoryCreateValidator : AbstractValidator<CategoryCreateDto>
{
    public CategoryCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Nome obrigatório");
        RuleFor(x => x.Description).MaximumLength(250);
    }
}

public class CategoryUpdateValidator : AbstractValidator<CategoryUpdateDto>
{
    public CategoryUpdateValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Description).MaximumLength(250);
    }
}
