using Library.Domain.Entities;
using Library.Domain.Repositories;
using Library.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories;

public sealed class BookRepository(LibraryDbContext context) : IBookRepository
{
    public async Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await BooksWithDetails()
            .OrderBy(book => book.Id)
            .ToListAsync(cancellationToken);

    public async Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await BooksWithDetails().FirstOrDefaultAsync(book => book.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Book>> GetByCategoryAsync(
        int categoryId, CancellationToken cancellationToken = default) =>
        await BooksWithDetails()
            .Where(book => book.CategoryId == categoryId)
            .OrderBy(book => book.Id)
            .ToListAsync(cancellationToken);

    private IQueryable<Book> BooksWithDetails() =>
        context.Books
            .AsNoTracking()
            .Include(book => book.Author)
            .Include(book => book.Category);
}
