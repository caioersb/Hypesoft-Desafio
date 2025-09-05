using Hypesoft.Application.Products.Commands;
using Hypesoft.Application.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hypesoft.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Mantém autenticação básica
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(string id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id));
        if (product is null) return NotFound();
        return Ok(product);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Search(
        [FromQuery] string? name,
        [FromQuery] string? categoryId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new SearchProductsQuery(name, categoryId, page, pageSize));
        return Ok(result);
    }

    [HttpGet("all")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var products = await _mediator.Send(new ListProductsQuery());
        return Ok(products);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
    {
        var product = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateProductCommand command)
    {
        var updatedCommand = command with { Dto = command.Dto with { Id = id } };
        await _mediator.Send(updatedCommand);
        return NoContent();
    }

    [HttpPost("{id}/adjust-stock")]
    [AllowAnonymous]
    public async Task<IActionResult> AdjustStock(string id, [FromBody] AdjustStockDto dto)
    {
        var command = new AdjustStockCommand(id, dto.Delta);
        var newQty = await _mediator.Send(command);
        return Ok(newQty);
    }

    [HttpDelete("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> Delete(string id)
    {
        await _mediator.Send(new DeleteProductCommand(id));
        return NoContent();
    }
}
