using Xunit;
using FluentAssertions;
using Moq;
using Hypesoft.Application.Products.Queries;
using Hypesoft.Domain.Repositories;
using Hypesoft.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Hypesoft.Tests.Application.Products
{
    public class GetProductByIdHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Return_Product_When_Found()
        {
            var product = new Product { Id = "1", Name = "Prod", Description = "Desc", Price = 100, CategoryId = "1", StockQuantity = 5 };
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(r => r.GetByIdAsync("1", It.IsAny<CancellationToken>())).ReturnsAsync(product);

            var handler = new GetProductByIdHandler(repoMock.Object);
            var query = new GetProductByIdQuery("1");

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result!.Name.Should().Be("Prod");
        }

        [Fact]
        public async Task Handle_Should_Return_Null_When_NotFound()
        {
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(r => r.GetByIdAsync("1", It.IsAny<CancellationToken>())).ReturnsAsync((Product?)null);

            var handler = new GetProductByIdHandler(repoMock.Object);
            var query = new GetProductByIdQuery("1");

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().BeNull();
        }
    }
}
