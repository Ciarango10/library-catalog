namespace Library.Application.Books.Dtos;

/// <summary>
/// Representación resumida de un libro para listados.
/// </summary>
public sealed record BookDto(
    int Id,
    string Title,
    string Isbn,
    int PublicationYear,
    string Author,
    string Category);
