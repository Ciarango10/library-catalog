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

    public Task<(IReadOnlyList<Book> Items, int TotalCount)> GetByCategoryAsync(
        int categoryId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        List<Book> booksInCategory = _books
            .Where(book => book.CategoryId == categoryId)
            .OrderBy(book => book.Id)
            .ToList();

        List<Book> page = booksInCategory
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Task.FromResult<(IReadOnlyList<Book> Items, int TotalCount)>((page, booksInCategory.Count));
    }

    /// <summary>
    /// Crea un libro con Id asignado (en producción lo asigna la base de datos).
    /// </summary>
    public static Book CreateBook(int id, string title, string isbn, int year, string author, string category) =>
        CreateBook(id, title, isbn, year, author, new Category(category));

    /// <summary>
    /// Crea un libro con Id asignado dentro de una categoría existente.
    /// </summary>
    public static Book CreateBook(int id, string title, string isbn, int year, string author, Category category) =>
        WithId(new Book(title, new Isbn(isbn), new PublicationYear(year), new Author(author), category), id);

    /// <summary>
    /// Crea una categoría con Id asignado (en producción lo asigna la base de datos).
    /// </summary>
    public static Category CreateCategory(int id, string name) => WithId(new Category(name), id);

    private static T WithId<T>(T entity, int id) where T : Entity
    {
        typeof(Entity).GetProperty(nameof(Entity.Id))!.SetValue(entity, id);
        return entity;
    }
}
