using Xunit;
using FluentAssertions;
using Moq;
using AutoMapper;
using Hypesoft.Application.Products.Queries;
using Hypesoft.Domain.Repositories;
using Hypesoft.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Hypesoft.Tests.Application.Products
{
    public class ListProductsHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Return_List_Of_Products()
        {
            var products = new List<Product> { new Product { Id = "1", Name = "Prod1" } };
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(r => r.ListAsync(It.IsAny<CancellationToken>())).ReturnsAsync(products);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<IReadOnlyList<Hypesoft.Application.DTOs.ProductReadDto>>(It.IsAny<IReadOnlyList<Product>>()))
                      .Returns((IReadOnlyList<Product> list) => new List<Hypesoft.Application.DTOs.ProductReadDto> { new Hypesoft.Application.DTOs.ProductReadDto { Id = list[0].Id, Name = list[0].Name } });

            var handler = new ListProductsHandler(repoMock.Object, mapperMock.Object);
            var query = new ListProductsQuery();

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().HaveCount(1);
            result[0].Name.Should().Be("Prod1");
        }
    }
}
