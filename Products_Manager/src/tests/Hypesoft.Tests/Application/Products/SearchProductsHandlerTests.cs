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
    public class SearchProductsHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Return_Products_With_Total()
        {
            var products = new List<Product> { new Product { Id = "1", Name = "Prod1" } };
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(r => r.SearchAsync("Prod1", null, 1, 20, It.IsAny<CancellationToken>()))
                    .ReturnsAsync((products, 1L));

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<IReadOnlyList<Hypesoft.Application.DTOs.ProductReadDto>>(It.IsAny<IReadOnlyList<Product>>()))
                      .Returns((IReadOnlyList<Product> list) => new List<Hypesoft.Application.DTOs.ProductReadDto> { new Hypesoft.Application.DTOs.ProductReadDto { Id = list[0].Id, Name = list[0].Name } });

            var handler = new SearchProductsHandler(repoMock.Object, mapperMock.Object);
            var query = new SearchProductsQuery("Prod1", null);

            var (items, total) = await handler.Handle(query, CancellationToken.None);

            items.Should().HaveCount(1);
            items[0].Name.Should().Be("Prod1");
            total.Should().Be(1);
        }
    }
}
