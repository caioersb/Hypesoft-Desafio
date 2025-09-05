using Xunit;
using FluentAssertions;
using Moq;
using Hypesoft.Application.Categories.Queries;
using Hypesoft.Domain.Repositories;
using Hypesoft.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Hypesoft.Tests.Application.Categories
{
    public class ListCategoriesHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Return_List_Of_Categories()
        {
            var categories = new List<Category> { new Category { Id = "1", Name = "Cat1", Description = "Desc1" } };
            var repoMock = new Mock<ICategoryRepository>();
            repoMock.Setup(r => r.ListAsync(It.IsAny<CancellationToken>())).ReturnsAsync(categories);

            var handler = new ListCategoriesHandler(repoMock.Object);
            var query = new ListCategoriesQuery();

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().HaveCount(1);
            result[0].Name.Should().Be("Cat1");
        }
    }
}
