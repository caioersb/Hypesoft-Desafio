using Xunit;
using FluentAssertions;
using Moq;
using Hypesoft.Application.Products.Commands;
using Hypesoft.Domain.Repositories;
using Hypesoft.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Hypesoft.Tests.Application.Products
{
    public class AdjustStockHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Adjust_Stock_Correctly()
        {
            // Arrange
            var product = new Product { Id = "1", StockQuantity = 5 };
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(r => r.GetByIdAsync("1", It.IsAny<CancellationToken>())).ReturnsAsync(product);
            repoMock.Setup(r => r.UpdateAsync(product, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var handler = new AdjustStockHandler(repoMock.Object);
            var command = new AdjustStockCommand { ProductId = "1", Delta = 10 };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(15);
            product.StockQuantity.Should().Be(15);
            repoMock.Verify(r => r.UpdateAsync(product, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Product_NotFound()
        {
            // Arrange
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(r => r.GetByIdAsync("1", It.IsAny<CancellationToken>())).ReturnsAsync((Product?)null);
            var handler = new AdjustStockHandler(repoMock.Object);

            var command = new AdjustStockCommand { ProductId = "1", Delta = 10 };

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Product not found");
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Resulting_Stock_Negative()
        {
            // Arrange
            var product = new Product { Id = "1", StockQuantity = 5 };
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(r => r.GetByIdAsync("1", It.IsAny<CancellationToken>())).ReturnsAsync(product);

            var handler = new AdjustStockHandler(repoMock.Object);
            var command = new AdjustStockCommand { ProductId = "1", Delta = -10 };

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Stock cannot be negative");
        }
    }
}
