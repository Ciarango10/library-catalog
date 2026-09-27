using Library.Application.Books.Dtos;
using Library.Domain.Repositories;
using MediatR;

namespace Library.Application.Books.Queries.GetBookById;

public sealed class GetBookByIdQueryHandler(IBookRepository bookRepository)
    : IRequestHandler<GetBookByIdQuery, BookDetailDto?>
{
    public async Task<BookDetailDto?> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = await bookRepository.GetByIdAsync(request.Id, cancellationToken);

        if (book is null)
            return null;

        return new BookDetailDto(
            book.Id,
            book.Title,
            book.Isbn.Value,
            book.PublicationYear.Value,
            new AuthorDto(book.Author.Id, book.Author.Name),
            new CategoryDto(book.Category.Id, book.Category.Name));
    }
}
