using Library.Application.Books.Dtos;
using MediatR;

namespace Library.Application.Books.Queries.GetAllBooks;

/// <summary>
/// Query para consultar todos los libros del catálogo.
/// </summary>
public sealed record GetAllBooksQuery : IRequest<IReadOnlyList<BookDto>>;
