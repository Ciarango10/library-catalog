using Library.Application.Books.Dtos;
using Library.Domain.Repositories;
using MediatR;

namespace Library.Application.Books.Queries.GetAllBooks;

public sealed class GetAllBooksQueryHandler(IBookRepository bookRepository)
    : IRequestHandler<GetAllBooksQuery, IReadOnlyList<BookDto>>
{
    public async Task<IReadOnlyList<BookDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        var books = await bookRepository.GetAllAsync(cancellationToken);

        return books
            .Select(book => new BookDto(
                book.Id,
                book.Title,
                book.Isbn.Value,
                book.PublicationYear.Value,
                book.Author.Name,
                book.Category.Name))
            .ToList();
    }
}
