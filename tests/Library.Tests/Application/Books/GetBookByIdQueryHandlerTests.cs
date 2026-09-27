using Library.Application.Books.Queries.GetBookById;
using Library.Tests.Application.Fakes;

namespace Library.Tests.Application.Books;

public class GetBookByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_WhenBookExists_ReturnsDetailWithAuthorAndCategory()
    {
        var book = FakeBookRepository.CreateBook(7, "Patrones de diseño", "9780000000040", 2014, "Autor Siete", "Tecnología");
        var handler = new GetBookByIdQueryHandler(new FakeBookRepository(book));

        var result = await handler.Handle(new GetBookByIdQuery(7), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(7, result.Id);
        Assert.Equal("Patrones de diseño", result.Title);
        Assert.Equal("9780000000040", result.Isbn);
        Assert.Equal(2014, result.PublicationYear);
        Assert.Equal("Autor Siete", result.Author.Name);
        Assert.Equal("Tecnología", result.Category.Name);
    }

    [Fact]
    public async Task Handle_WhenBookDoesNotExist_ReturnsNull()
    {
        var book = FakeBookRepository.CreateBook(7, "Patrones de diseño", "9780000000040", 2014, "Autor Siete", "Tecnología");
        var handler = new GetBookByIdQueryHandler(new FakeBookRepository(book));

        var result = await handler.Handle(new GetBookByIdQuery(999), CancellationToken.None);

        Assert.Null(result);
    }
}
