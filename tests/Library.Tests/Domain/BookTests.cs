using Library.Domain.Common;
using Library.Domain.Entities;
using Library.Domain.Exceptions;
using Library.Domain.ValueObjects;

namespace Library.Tests.Domain;

public class BookTests
{
    private static readonly Isbn ValidIsbn = new("9780132350884");
    private static readonly PublicationYear ValidYear = new(2008);

    private static Author NewAuthor() => new("Robert C. Martin");

    private static Category NewCategory() => new("Software");

    [Fact]
    public void Constructor_WithValidData_SetsAllProperties()
    {
        var author = NewAuthor();
        var category = NewCategory();

        var book = new Book("  Clean Code  ", ValidIsbn, ValidYear, author, category);

        Assert.Equal("Clean Code", book.Title);
        Assert.Equal(ValidIsbn, book.Isbn);
        Assert.Equal(ValidYear, book.PublicationYear);
        Assert.Same(author, book.Author);
        Assert.Same(category, book.Category);
        Assert.Equal(author.Id, book.AuthorId);
        Assert.Equal(category.Id, book.CategoryId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidTitle_ThrowsDomainException(string? title)
    {
        Assert.Throws<DomainException>(() =>
            new Book(title!, ValidIsbn, ValidYear, NewAuthor(), NewCategory()));
    }

    [Fact]
    public void Constructor_WithNullDependencies_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new Book("Clean Code", null!, ValidYear, NewAuthor(), NewCategory()));
        Assert.Throws<ArgumentNullException>(() =>
            new Book("Clean Code", ValidIsbn, null!, NewAuthor(), NewCategory()));
        Assert.Throws<ArgumentNullException>(() =>
            new Book("Clean Code", ValidIsbn, ValidYear, null!, NewCategory()));
        Assert.Throws<ArgumentNullException>(() =>
            new Book("Clean Code", ValidIsbn, ValidYear, NewAuthor(), null!));
    }

    [Fact]
    public void Constructor_RegistersBookInAuthorAndCategoryCollections()
    {
        var author = NewAuthor();
        var category = NewCategory();

        var book = new Book("Clean Code", ValidIsbn, ValidYear, author, category);

        Assert.Same(book, Assert.Single(author.Books));
        Assert.Same(book, Assert.Single(category.Books));
    }

    [Fact]
    public void AuthorAndCategoryBooks_AreReadOnlyCollections()
    {
        var author = NewAuthor();
        var category = NewCategory();
        _ = new Book("Clean Code", ValidIsbn, ValidYear, author, category);

        Assert.True(((ICollection<Book>)author.Books).IsReadOnly);
        Assert.True(((ICollection<Book>)category.Books).IsReadOnly);
    }

    [Theory]
    [InlineData(typeof(Book))]
    [InlineData(typeof(Author))]
    [InlineData(typeof(Category))]
    public void Entities_InheritFromEntity_AndExposeNoPublicSetters(Type entityType)
    {
        Assert.True(typeof(Entity).IsAssignableFrom(entityType));

        var propertiesWithPublicSetter = entityType
            .GetProperties()
            .Where(p => p.SetMethod?.IsPublic == true)
            .Select(p => p.Name);

        Assert.Empty(propertiesWithPublicSetter);
    }
}
