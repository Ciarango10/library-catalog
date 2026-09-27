namespace Library.Application.Books.Dtos;

/// <summary>
/// Representación detallada de un libro, con la información
/// de su autor y de su categoría.
/// </summary>
public sealed record BookDetailDto(
    int Id,
    string Title,
    string Isbn,
    int PublicationYear,
    AuthorDto Author,
    CategoryDto Category);

public sealed record AuthorDto(int Id, string Name);

public sealed record CategoryDto(int Id, string Name);
