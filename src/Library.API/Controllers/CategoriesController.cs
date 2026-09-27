using Library.Application.Books.Dtos;
using Library.Application.Books.Queries.GetBooksByCategory;
using Library.Application.Utilities.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Consulta de forma paginada los libros que pertenecen a una categoría.
    /// </summary>
    /// <param name="categoryId">Identificador de la categoría.</param>
    /// <param name="pageNumber">Número de página (mínimo 1).</param>
    /// <param name="pageSize">Libros por página (máximo 50).</param>
    /// <param name="cancellationToken">Token de cancelación de la petición.</param>
    /// <response code="200">Página de libros de la categoría (vacía si la categoría no tiene libros).</response>
    /// <response code="404">No existe una categoría con ese identificador.</response>
    [HttpGet("{categoryId:int}/books")]
    [ProducesResponseType(typeof(PaginationResponse<BookDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PaginationResponse<BookDto>>> GetBooks(
        int categoryId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = PaginationRequest.DEFAULT_PAGE_SIZE,
        CancellationToken cancellationToken = default)
    {
        GetBooksByCategoryQuery query = new(categoryId)
        {
            Pagination = new PaginationRequest(pageNumber, pageSize)
        };

        PaginationResponse<BookDto>? books = await sender.Send(query, cancellationToken);

        if (books is null)
            return NotFound();

        return Ok(books);
    }
}
