using Library.Domain.Common;
using Library.Domain.Exceptions;

namespace Library.Domain.Entities;

public class Category : Entity
{
    private readonly List<Book> _books = new();

    public string Name { get; private set; }

    public IReadOnlyCollection<Book> Books => _books.AsReadOnly();

    // Requerido por EF Core para materializar la entidad
    private Category()
    {
        Name = null!;
    }

    public Category(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("El nombre de la categoría es obligatorio.");

        Name = name.Trim();
    }

    internal void AddBook(Book book) => _books.Add(book);
}
