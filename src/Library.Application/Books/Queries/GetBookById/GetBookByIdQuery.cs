using Library.Application.Books.Dtos;
using MediatR;

namespace Library.Application.Books.Queries.GetBookById;

/// <summary>
/// Query para consultar un libro por su identificador.
/// Retorna null si el libro no existe.
/// </summary>
public sealed record GetBookByIdQuery(int Id) : IRequest<BookDetailDto?>;
