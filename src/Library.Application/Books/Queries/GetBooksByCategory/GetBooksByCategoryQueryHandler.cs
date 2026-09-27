using Library.Application.Books.Dtos;
using Library.Application.Books.Mappers;
using Library.Application.Utilities.Pagination;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using MediatR;

namespace Library.Application.Books.Queries.GetBooksByCategory;

public sealed class GetBooksByCategoryQueryHandler(
    IBookRepository bookRepository,
    ICategoryRepository categoryRepository)
    : IRequestHandler<GetBooksByCategoryQuery, PaginationResponse<BookDto>?>
{
    public async Task<PaginationResponse<BookDto>?> Handle(
        GetBooksByCategoryQuery request, CancellationToken cancellationToken)
    {
        bool categoryExists = await categoryRepository.ExistsAsync(request.CategoryId, cancellationToken);

        if (!categoryExists)
            return null;

        PaginationRequest pagination = request.Pagination;

        (IReadOnlyList<Book> books, int totalCount) = await bookRepository.GetByCategoryAsync(
            request.CategoryId, pagination.PageNumber, pagination.PageSize, cancellationToken);

        List<BookDto> items = books.Select(book => book.ToBookDto()).ToList();

        return PaginationResponse<BookDto>.Create(items, totalCount, pagination);
    }
}
