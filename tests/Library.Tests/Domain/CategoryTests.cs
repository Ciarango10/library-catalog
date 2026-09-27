using Library.Domain.Entities;
using Library.Domain.Exceptions;

namespace Library.Tests.Domain;

public class CategoryTests
{
    [Fact]
    public void Constructor_WithValidName_SetsNameTrimmed()
    {
        var category = new Category("  Novela  ");

        Assert.Equal("Novela", category.Name);
        Assert.Empty(category.Books);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidName_ThrowsDomainException(string? name)
    {
        Assert.Throws<DomainException>(() => new Category(name!));
    }
}
