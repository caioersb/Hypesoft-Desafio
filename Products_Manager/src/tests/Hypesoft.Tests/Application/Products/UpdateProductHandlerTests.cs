using Xunit;
using FluentAssertions;
using Moq;
using AutoMapper;
using Hypesoft.Application.Products.Handlers;
using Hypesoft.Application.Products.Commands;
using Hypesoft.Application.DTOs;
using Hypesoft.Domain.Repositories;
using Hypesoft.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Hypesoft.Tests.Application.Products
{
    public class UpdateProductHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Update_Product_And_Return_DTO()
        {
            // Arrange
            var product = new Product { Id = "1", Name = "Old", Description = "Old Desc", Price = 50, StockQuantity = 5, CategoryId = "1" };
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(r => r.GetByIdAsync("1", It.IsAny<CancellationToken>())).ReturnsAsync(product);
            repoMock.Setup(r => r.UpdateAsync(product, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<ProductReadDto>(It.IsAny<Product>())).Returns((Product p) => new ProductReadDto { Id = p.Id, Name = p.Name });

            var handler = new UpdateProductHandler(repoMock.Object, mapperMock.Object);

            var command = new UpdateProductCommand
            {
                Dto = new ProductUpdateDto
                {
                    Id = "1",
                    Name = "New",
                    Description = "New Desc",
                    Price = 100,
                    StockQuantity = 10,
                    CategoryId = "1"
                }
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Name.Should().Be("New");
            product.Price.Should().Be(100);
            product.StockQuantity.Should().Be(10);
            repoMock.Verify(r => r.UpdateAsync(product, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Product_NotFound()
        {
            // Arrange
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(r => r.GetByIdAsync("1", It.IsAny<CancellationToken>())).ReturnsAsync((Product?)null);
            var mapperMock = new Mock<IMapper>();
            var handler = new UpdateProductHandler(repoMock.Object, mapperMock.Object);

            var command = new UpdateProductCommand
            {
                Dto = new ProductUpdateDto { Id = "1" }
            };

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Product not found");
        }
    }
}
