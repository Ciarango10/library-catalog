using Library.Domain.Entities;
using Library.Domain.Exceptions;

namespace Library.Tests.Domain;

public class AuthorTests
{
    [Fact]
    public void Constructor_WithValidName_SetsNameTrimmed()
    {
        var author = new Author("  Gabriel García Márquez  ");

        Assert.Equal("Gabriel García Márquez", author.Name);
        Assert.Empty(author.Books);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidName_ThrowsDomainException(string? name)
    {
        Assert.Throws<DomainException>(() => new Author(name!));
    }
}
