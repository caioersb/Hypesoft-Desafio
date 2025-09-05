using Xunit;
using FluentAssertions;
using Hypesoft.Domain.Entities;

namespace Hypesoft.Tests.Domain.Entities
{
    public class CategoryTests
    {
        [Fact]
        public void Should_Create_Category_With_Valid_Properties()
        {
            // Arrange
            var category = new Category
            {
                Id = "1",
                Name = "Eletrônicos",
                Description = "Categoria de produtos eletrônicos"
            };

            // Assert
            category.Id.Should().Be("1");
            category.Name.Should().Be("Eletrônicos");
            category.Description.Should().Be("Categoria de produtos eletrônicos");
        }
    }
}
