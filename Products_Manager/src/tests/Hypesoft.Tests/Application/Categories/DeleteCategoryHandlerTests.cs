using Xunit;
using FluentAssertions;
using Moq;
using Hypesoft.Application.Categories.Handlers;
using Hypesoft.Application.Categories.Commands;
using Hypesoft.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Hypesoft.Tests.Application.Categories
{
    public class DeleteCategoryHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Call_Repo_DeleteAsync()
        {
            // Arrange
            var repoMock = new Mock<ICategoryRepository>();
            repoMock.Setup(r => r.DeleteAsync("1", It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var handler = new DeleteCategoryHandler(repoMock.Object);
            var command = new DeleteCategoryCommand { Id = "1" };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            repoMock.Verify(r => r.DeleteAsync("1", It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
