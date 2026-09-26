using Library.Domain.Common;
using Library.Domain.Exceptions;
using Library.Domain.ValueObjects;

namespace Library.Domain.Entities;

public class Book : Entity
{
    public string Title { get; private set; }
    public Isbn Isbn { get; private set; }
    public PublicationYear PublicationYear { get; private set; }
    public int AuthorId { get; private set; }
    public Author Author { get; private set; }
    public int CategoryId { get; private set; }
    public Category Category { get; private set; }

    // Requerido por EF Core para materializar la entidad
    private Book()
    {
        Title = null!;
        Isbn = null!;
        PublicationYear = null!;
        Author = null!;
        Category = null!;
    }

    public Book(string title, Isbn isbn, PublicationYear publicationYear, Author author, Category category)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("El título del libro es obligatorio.");

        ArgumentNullException.ThrowIfNull(isbn);
        ArgumentNullException.ThrowIfNull(publicationYear);
        ArgumentNullException.ThrowIfNull(author);
        ArgumentNullException.ThrowIfNull(category);

        Title = title.Trim();
        Isbn = isbn;
        PublicationYear = publicationYear;
        Author = author;
        AuthorId = author.Id;
        Category = category;
        CategoryId = category.Id;

        author.AddBook(this);
        category.AddBook(this);
    }
}
