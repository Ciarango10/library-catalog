using Library.Application.Books.Dtos;
using Library.Application.Books.Queries.GetAllBooks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Consulta todos los libros del catálogo.
    /// </summary>
    /// <response code="200">Lista de libros (vacía si no hay libros).</response>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<BookDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<BookDto>>> GetAll(CancellationToken cancellationToken)
    {
        var books = await sender.Send(new GetAllBooksQuery(), cancellationToken);
        return Ok(books);
    }
}
