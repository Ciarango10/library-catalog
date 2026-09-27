using Library.Application.Books.Dtos;
using Library.Domain.Entities;

namespace Library.Application.Books.Mappers;

/// <summary>
/// Transformaciones de la entidad Book hacia los DTOs expuestos por los casos de uso.
/// </summary>
internal static class BookMapperExtensions
{
    public static BookDto ToBookDto(this Book book) =>
        new(
            book.Id,
            book.Title,
            book.Isbn.Value,
            book.PublicationYear.Value,
            book.Author.Name,
            book.Category.Name);
}
