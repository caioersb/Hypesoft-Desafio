using System.Net;
using System.Net.Http.Json;
using Hypesoft.API;
using Hypesoft.Application.Categories.Commands;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using FluentAssertions;

namespace Hypesoft.Tests.Integration.Controllers;

public class CategoryControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CategoryControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Create_Get_Delete_Category_Should_Work()
    {
        // 1. Criar
        var createCommand = new CreateCategoryCommand
        {
            Dto = new() { Name = "Teste", Description = "Categoria teste" }
        };
        var createResponse = await _client.PostAsJsonAsync("/api/Category", createCommand);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var id = createResponse.Headers.Location!.Segments.Last();

        // 2. Buscar pelo id
        var getResponse = await _client.GetAsync($"/api/Category/{id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var category = await getResponse.Content.ReadFromJsonAsync<dynamic>();
        ((string)category.name).Should().Be("Teste");

        // 3. Deletar
        var deleteResponse = await _client.DeleteAsync($"/api/Category/{id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
