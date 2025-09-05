using Xunit;
using FluentAssertions;
using Moq;
using Hypesoft.Application.Categories.Queries;
using Hypesoft.Domain.Repositories;
using Hypesoft.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Hypesoft.Tests.Application.Categories
{
    public class GetCategoryByIdHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Return_Category_When_Found()
        {
            var category = new Category { Id = "1", Name = "Cat", Description = "Desc" };
            var repoMock = new Mock<ICategoryRepository>();
            repoMock.Setup(r => r.GetByIdAsync("1", It.IsAny<CancellationToken>())).ReturnsAsync(category);

            var handler = new GetCategoryByIdHandler(repoMock.Object);
            var query = new GetCategoryByIdQuery("1");

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result!.Name.Should().Be("Cat");
        }

        [Fact]
        public async Task Handle_Should_Return_Null_When_NotFound()
        {
            var repoMock = new Mock<ICategoryRepository>();
            repoMock.Setup(r => r.GetByIdAsync("1", It.IsAny<CancellationToken>())).ReturnsAsync((Category?)null);

            var handler = new GetCategoryByIdHandler(repoMock.Object);
            var query = new GetCategoryByIdQuery("1");

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().BeNull();
        }
    }
}
