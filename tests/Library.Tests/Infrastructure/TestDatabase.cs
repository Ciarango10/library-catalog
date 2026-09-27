using Library.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Library.Tests.Infrastructure;

/// <summary>
/// Base de datos SQLite en memoria con el esquema y los datos semilla del catálogo.
/// </summary>
internal sealed class TestDatabase : IAsyncDisposable
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
        SqliteConnection connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        DbContextOptions<LibraryDbContext> options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseSqlite(connection)
            .Options;
        LibraryDbContext context = new LibraryDbContext(options);
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
