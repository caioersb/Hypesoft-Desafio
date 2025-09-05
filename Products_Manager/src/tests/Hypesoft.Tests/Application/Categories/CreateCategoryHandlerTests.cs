using Xunit;
using FluentAssertions;
using Moq;
using Hypesoft.Application.Categories.Handlers;
using Hypesoft.Application.Categories.Commands;
using Hypesoft.Domain.Repositories;
using Hypesoft.Application.DTOs;
using System.Threading;
using System.Threading.Tasks;

namespace Hypesoft.Tests.Application.Categories
{
    public class CreateCategoryHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Call_Repo_AddAsync_And_Return_Id()
        {
            // Arrange
            var repoMock = new Mock<ICategoryRepository>();
            repoMock.Setup(r => r.AddAsync(It.IsAny<Domain.Entities.Category>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

            var handler = new CreateCategoryHandler(repoMock.Object);

            var command = new CreateCategoryCommand
            {
                Dto = new CategoryDTO { Name = "Eletrônicos", Description = "Descrição" }
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            repoMock.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.Category>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
