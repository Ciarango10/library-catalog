using Library.Domain.Common;
using Library.Domain.Exceptions;

namespace Library.Domain.Entities;

public class Author : Entity
{
    private readonly List<Book> _books = new();

    public string Name { get; private set; }

    public IReadOnlyCollection<Book> Books => _books.AsReadOnly();

    // Requerido por EF Core para materializar la entidad
    private Author()
    {
        Name = null!;
    }

    public Author(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("El nombre del autor es obligatorio.");

        Name = name.Trim();
    }

    internal void AddBook(Book book) => _books.Add(book);
}
