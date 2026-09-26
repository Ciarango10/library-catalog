using Library.Domain.Entities;
using Library.Infrastructure.Persistence;
using Library.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

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

        var books = await repository.GetByCategoryAsync(2);
        var missing = await repository.GetByCategoryAsync(999);

        Assert.Equal(3, books.Count);
        Assert.All(books, book =>
        {
            Assert.Equal(2, book.CategoryId);
            AssertDetailsLoaded(book);
        });
        Assert.Empty(missing);
        Assert.Empty(database.Context.ChangeTracker.Entries());
    }

    private static void AssertDetailsLoaded(Book book)
    {
        Assert.False(string.IsNullOrWhiteSpace(book.Author.Name));
        Assert.False(string.IsNullOrWhiteSpace(book.Category.Name));
    }

    private sealed class TestDatabase : IAsyncDisposable
    {
        private readonly SqliteConnection _connection;

        private TestDatabase(SqliteConnection connection, LibraryDbContext context)
        {
            _connection = connection;
            Context = context;
        }

        public LibraryDbContext Context { get; }

        public static async Task<TestDatabase> CreateAsync()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseSqlite(connection)
                .Options;
            var context = new LibraryDbContext(options);
            await context.Database.EnsureCreatedAsync();
            context.ChangeTracker.Clear();
            return new TestDatabase(connection, context);
        }

        public async ValueTask DisposeAsync()
        {
            await Context.DisposeAsync();
            await _connection.DisposeAsync();
        }
    }
}
