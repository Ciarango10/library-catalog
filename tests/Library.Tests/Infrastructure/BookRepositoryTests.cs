using Library.Domain.Entities;
using Library.Infrastructure.Repositories;

namespace Library.Tests.Infrastructure;

public sealed class BookRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_ReturnsSeededBooksWithDetailsWithoutTracking()
    {
        await using var database = await TestDatabase.CreateAsync();
        var repository = new BookRepository(database.Context);

        var books = await repository.GetAllAsync();

        Assert.Equal(10, books.Count);
        Assert.All(books, AssertDetailsLoaded);
        Assert.Empty(database.Context.ChangeTracker.Entries());
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsMatchingBookOrNull()
    {
        await using var database = await TestDatabase.CreateAsync();
        var repository = new BookRepository(database.Context);

        var book = await repository.GetByIdAsync(1);
        var missing = await repository.GetByIdAsync(999);

        Assert.NotNull(book);
        Assert.Equal("Arquitectura de software", book.Title);
        Assert.Equal("9780000000019", book.Isbn.Value);
        Assert.Equal(2018, book.PublicationYear.Value);
        AssertDetailsLoaded(book);
        Assert.Null(missing);
        Assert.Empty(database.Context.ChangeTracker.Entries());
    }

    [Fact]
    public async Task GetByCategoryAsync_FiltersBooksAndLoadsDetails()
    {
        await using var database = await TestDatabase.CreateAsync();
        var repository = new BookRepository(database.Context);

        var (books, totalCount) = await repository.GetByCategoryAsync(2, pageNumber: 1, pageSize: 10);
        var (missing, missingCount) = await repository.GetByCategoryAsync(999, pageNumber: 1, pageSize: 10);

        Assert.Equal(3, books.Count);
        Assert.Equal(3, totalCount);
        Assert.All(books, book =>
        {
            Assert.Equal(2, book.CategoryId);
            AssertDetailsLoaded(book);
        });
        Assert.Empty(missing);
        Assert.Equal(0, missingCount);
        Assert.Empty(database.Context.ChangeTracker.Entries());
    }

    [Fact]
    public async Task GetByCategoryAsync_ReturnsOnlyTheRequestedPageOrderedById()
    {
        await using var database = await TestDatabase.CreateAsync();
        var repository = new BookRepository(database.Context);

        var (firstPage, totalCount) = await repository.GetByCategoryAsync(1, pageNumber: 1, pageSize: 3);
        var (secondPage, _) = await repository.GetByCategoryAsync(1, pageNumber: 2, pageSize: 3);
        var (emptyPage, _) = await repository.GetByCategoryAsync(1, pageNumber: 3, pageSize: 3);

        Assert.Equal(4, totalCount);
        Assert.Equal([1, 2, 3], firstPage.Select(book => book.Id));
        Assert.Equal([4], secondPage.Select(book => book.Id));
        Assert.Empty(emptyPage);
    }

    private static void AssertDetailsLoaded(Book book)
    {
        Assert.False(string.IsNullOrWhiteSpace(book.Author.Name));
        Assert.False(string.IsNullOrWhiteSpace(book.Category.Name));
    }
}
