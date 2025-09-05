using Xunit;
using FluentAssertions;
using Moq;
using Hypesoft.Application.Products.Handlers;
using Hypesoft.Application.Products.Commands;
using Hypesoft.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Hypesoft.Tests.Application.Products
{
    public class DeleteProductHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Call_DeleteAsync()
        {
            // Arrange
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(r => r.DeleteAsync("1", It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var handler = new DeleteProductHandler(repoMock.Object);
            var command = new DeleteProductCommand { Id = "1" };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            repoMock.Verify(r => r.DeleteAsync("1", It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
