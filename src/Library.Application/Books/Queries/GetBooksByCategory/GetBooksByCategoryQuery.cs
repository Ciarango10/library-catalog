using Library.Application.Books.Dtos;
using Library.Application.Utilities.Pagination;
using MediatR;

namespace Library.Application.Books.Queries.GetBooksByCategory;

/// <summary>
/// Query para consultar, de forma paginada, los libros que pertenecen a una categoría.
/// Retorna null si la categoría no existe y una página vacía si
/// la categoría existe pero no tiene libros.
/// </summary>
public sealed record GetBooksByCategoryQuery(int CategoryId) : IRequest<PaginationResponse<BookDto>?>
{
    public PaginationRequest Pagination { get; init; } = PaginationRequest.Default();
}
