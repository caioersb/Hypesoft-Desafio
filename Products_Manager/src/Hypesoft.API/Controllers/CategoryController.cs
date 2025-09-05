using Hypesoft.Application.Categories.Commands;
using Hypesoft.Application.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hypesoft.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Mantém autenticação básica
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    [AllowAnonymous] // liberado geral
    public async Task<IActionResult> GetById(string id)
    {
        var category = await _mediator.Send(new GetCategoryByIdQuery(id));
        if (category is null) return NotFound();
        return Ok(category);
    }

    [HttpGet]
    [AllowAnonymous] // liberado geral
    public async Task<IActionResult> GetAll()
    {
        var categories = await _mediator.Send(new ListCategoriesQuery());
        return Ok(categories);
    }

    [HttpPost]
    [AllowAnonymous] // liberado geral
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpPut("{id}")]
    [AllowAnonymous] // liberado geral
    public async Task<IActionResult> Update(string id, [FromBody] UpdateCategoryCommand command)
    {
        var updatedCommand = command with { Dto = command.Dto with { Id = id } };
        await _mediator.Send(updatedCommand);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [AllowAnonymous] // liberado geral
    public async Task<IActionResult> Delete(string id)
    {
        await _mediator.Send(new DeleteCategoryCommand(id));
        return NoContent();
    }
}
