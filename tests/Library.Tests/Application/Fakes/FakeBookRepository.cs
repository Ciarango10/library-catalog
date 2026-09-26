using Library.Domain.Common;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using Library.Domain.ValueObjects;

namespace Library.Tests.Application.Fakes;

internal sealed class FakeBookRepository(params Book[] books) : IBookRepository
{
    private readonly List<Book> _books = books.ToList();

    public Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<Book>>(_books.ToList());

    public Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        Task.FromResult(_books.FirstOrDefault(book => book.Id == id));

    public Task<IReadOnlyList<Book>> GetByCategoryAsync(int categoryId, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<Book>>(_books.Where(book => book.CategoryId == categoryId).ToList());

    /// <summary>
    /// Crea un libro con Id asignado (en producción lo asigna la base de datos).
    /// </summary>
    public static Book CreateBook(int id, string title, string isbn, int year, string author, string category)
    {
        var book = new Book(title, new Isbn(isbn), new PublicationYear(year), new Author(author), new Category(category));
        typeof(Entity).GetProperty(nameof(Entity.Id))!.SetValue(book, id);
        return book;
    }
}
