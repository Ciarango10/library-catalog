using Library.Application.Books.Dtos;
using Library.Application.Books.Queries.GetAllBooks;
using Library.Application.Books.Queries.GetBookById;
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

    /// <summary>
    /// Consulta un libro por su identificador, con la información de su autor y su categoría.
    /// </summary>
    /// <response code="200">El libro existe.</response>
    /// <response code="404">No existe un libro con ese identificador.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BookDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookDetailDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var book = await sender.Send(new GetBookByIdQuery(id), cancellationToken);

        if (book is null)
            return NotFound();

        return Ok(book);
    }
}
