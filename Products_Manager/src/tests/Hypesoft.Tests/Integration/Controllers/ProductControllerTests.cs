using System.Net;
using System.Net.Http.Json;
using Hypesoft.API;
using Hypesoft.Application.Products.Commands;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using FluentAssertions;

namespace Hypesoft.Tests.Integration.Controllers;

public class ProductControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProductControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Create_GetAll_AdjustStock_Delete_Product_Should_Work()
    {
        // 1. Criar produto
        var createCommand = new CreateProductCommand
        {
            Dto = new()
            {
                Name = "Produto Teste",
                Description = "Descrição",
                Price = 10.5m,
                StockQuantity = 5,
                CategoryId = "fake-category-id"
            }
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Product", createCommand);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var product = await createResponse.Content.ReadFromJsonAsync<dynamic>();
        string id = product.id;

        // 2. Buscar todos
        var getAllResponse = await _client.GetAsync("/api/Product/all");
        getAllResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // 3. Ajustar estoque
        var adjustResponse = await _client.PostAsJsonAsync($"/api/Product/{id}/adjust-stock", new { Delta = 3 });
        adjustResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var newQty = await adjustResponse.Content.ReadFromJsonAsync<int>();
        newQty.Should().Be(8);

        // 4. Deletar produto
        var deleteResponse = await _client.DeleteAsync($"/api/Product/{id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
