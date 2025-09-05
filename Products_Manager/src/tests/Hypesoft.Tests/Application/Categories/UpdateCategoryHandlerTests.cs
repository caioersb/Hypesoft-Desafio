using Xunit;
using FluentAssertions;
using Moq;
using Hypesoft.Application.Categories.Handlers;
using Hypesoft.Application.Categories.Commands;
using Hypesoft.Domain.Repositories;
using Hypesoft.Application.DTOs;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Hypesoft.Tests.Application.Categories
{
    public class UpdateCategoryHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Update_Category()
        {
            // Arrange
            var category = new Domain.Entities.Category { Id = "1", Name = "Old", Description = "Old Desc" };
            var repoMock = new Mock<ICategoryRepository>();
            repoMock.Setup(r => r.GetByIdAsync("1", It.IsAny<CancellationToken>())).ReturnsAsync(category);
            repoMock.Setup(r => r.UpdateAsync(category, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var handler = new UpdateCategoryHandler(repoMock.Object);
            var command = new UpdateCategoryCommand
            {
                Dto = new CategoryDTO { Id = "1", Name = "New", Description = "New Desc" }
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            category.Name.Should().Be("New");
            category.Description.Should().Be("New Desc");
            repoMock.Verify(r => r.UpdateAsync(category, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Category_NotFound()
        {
            // Arrange
            var repoMock = new Mock<ICategoryRepository>();
            repoMock.Setup(r => r.GetByIdAsync("1", It.IsAny<CancellationToken>())).ReturnsAsync((Domain.Entities.Category?)null);
            var handler = new UpdateCategoryHandler(repoMock.Object);

            var command = new UpdateCategoryCommand
            {
                Dto = new CategoryDTO { Id = "1", Name = "New", Description = "New Desc" }
            };

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Category not found");
        }
    }
}
