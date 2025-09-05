using MediatR;

namespace Hypesoft.Application.Categories.Commands
{
    public record DeleteCategoryCommand(string Id) : IRequest<Unit>;
}
