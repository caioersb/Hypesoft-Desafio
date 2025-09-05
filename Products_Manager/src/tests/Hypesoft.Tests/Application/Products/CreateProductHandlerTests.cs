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

namespace Hypesoft.Tests.Application.Products
{
    public class CreateProductHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Create_Product_And_Return_DTO()
        {
            // Arrange
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<ProductReadDto>(It.IsAny<Product>()))
                      .Returns((Product p) => new ProductReadDto
                      {
                          Id = p.Id,
                          Name = p.Name,
                          Description = p.Description,
                          Price = p.Price,
                          CategoryId = p.CategoryId,
                          StockQuantity = p.StockQuantity
                      });

            var handler = new CreateProductHandler(repoMock.Object, mapperMock.Object);

            var command = new CreateProductCommand
            {
                Dto = new ProductCreateDto
                {
                    Name = "Produto X",
                    Description = "Descrição",
                    Price = 100,
                    CategoryId = "1",
                    StockQuantity = 10
                }
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Produto X");
            repoMock.Verify(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
