using Xunit;
using FluentAssertions;
using Hypesoft.Domain.Entities;
using System;

namespace Hypesoft.Tests.Domain.Entities
{
    public class ProductTests
    {
        [Fact]
        public void Should_Create_Product_With_Valid_Properties()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var product = new Product
            {
                Id = "abc123",
                Name = "Smartphone",
                Description = "Smartphone top de linha",
                Price = 1999.99m,
                CategoryId = "1",
                StockQuantity = 50,
                CreatedAt = now,
                UpdatedAt = now
            };

            // Assert
            product.Id.Should().Be("abc123");
            product.Name.Should().Be("Smartphone");
            product.Description.Should().Be("Smartphone top de linha");
            product.Price.Should().Be(1999.99m);
            product.CategoryId.Should().Be("1");
            product.StockQuantity.Should().Be(50);
            product.CreatedAt.Should().Be(now);
            product.UpdatedAt.Should().Be(now);
        }

        [Fact]
        public void Should_Have_Default_Description_EmptyString()
        {
            // Arrange
            var product = new Product
            {
                Id = "abc123",
                Name = "Produto Teste",
                CategoryId = "1"
            };

            // Assert
            product.Description.Should().BeEmpty();
        }

        [Fact]
        public void Should_Have_Default_CreatedAt_And_UpdatedAt_As_UtcNow()
        {
            // Arrange
            var before = DateTime.UtcNow;
            var product = new Product
            {
                Id = "abc123",
                Name = "Produto Teste",
                CategoryId = "1"
            };
            var after = DateTime.UtcNow;

            // Assert
            product.CreatedAt.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
            product.UpdatedAt.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
        }
    }
}
