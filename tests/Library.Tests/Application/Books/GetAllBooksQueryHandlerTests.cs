using Library.Application.Books.Dtos;
using Library.Application.Books.Queries.GetAllBooks;
using Library.Tests.Application.Fakes;

namespace Library.Tests.Application.Books;

public class GetAllBooksQueryHandlerTests
{
    [Fact]
    public async Task Handle_WhenThereAreNoBooks_ReturnsEmptyList()
    {
        var handler = new GetAllBooksQueryHandler(new FakeBookRepository());

        var result = await handler.Handle(new GetAllBooksQuery(), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_WhenThereAreBooks_ReturnsBookDtosWithTheSixFields()
    {
        var repository = new FakeBookRepository(
            FakeBookRepository.CreateBook(1, "Arquitectura de software", "9780000000019", 2018, "Autor Uno", "Tecnología"),
            FakeBookRepository.CreateBook(2, "Historia de la ciencia", "9780000000057", 2012, "Autor Dos", "Ciencia"));
        var handler = new GetAllBooksQueryHandler(repository);

        var result = await handler.Handle(new GetAllBooksQuery(), CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Equal(new BookDto(1, "Arquitectura de software", "9780000000019", 2018, "Autor Uno", "Tecnología"), result[0]);
        Assert.Equal(new BookDto(2, "Historia de la ciencia", "9780000000057", 2012, "Autor Dos", "Ciencia"), result[1]);
    }
}
