using Library.Application.Books.Dtos;
using Library.Application.Books.Queries.GetBooksByCategory;
using Library.Application.Utilities.Pagination;
using Library.Domain.Entities;
using Library.Tests.Application.Fakes;

namespace Library.Tests.Application.Books;

public class GetBooksByCategoryQueryHandlerTests
{
    [Fact]
    public async Task Handle_WhenCategoryHasBooks_ReturnsOnlyTheBooksOfThatCategory()
    {
        Category technology = FakeBookRepository.CreateCategory(1, "Tecnología");
        Category science = FakeBookRepository.CreateCategory(2, "Ciencias");
        FakeBookRepository repository = new FakeBookRepository(
            FakeBookRepository.CreateBook(1, "Arquitectura de software", "9780000000019", 2018, "Autor Uno", technology),
            FakeBookRepository.CreateBook(5, "Historia de la ciencia", "9780000000057", 2012, "Autor Dos", science),
            FakeBookRepository.CreateBook(4, "Patrones de diseño", "9780000000040", 2014, "Autor Tres", technology));
        GetBooksByCategoryQueryHandler handler = new GetBooksByCategoryQueryHandler(repository, new FakeCategoryRepository(1, 2));

        PaginationResponse<BookDto>? result = await handler.Handle(new GetBooksByCategoryQuery(1), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(2, result.TotalCount);
        Assert.Equal(2, result.Items.Count);
        Assert.Equal(new BookDto(1, "Arquitectura de software", "9780000000019", 2018, "Autor Uno", "Tecnología"), result.Items[0]);
        Assert.Equal(new BookDto(4, "Patrones de diseño", "9780000000040", 2014, "Autor Tres", "Tecnología"), result.Items[1]);
    }

    [Fact]
    public async Task Handle_WhenPaginationIsNotSent_UsesTheFirstPageWithTheDefaultSize()
    {
        GetBooksByCategoryQueryHandler handler = new GetBooksByCategoryQueryHandler(new FakeBookRepository(), new FakeCategoryRepository(1));

        PaginationResponse<BookDto>? result = await handler.Handle(new GetBooksByCategoryQuery(1), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(PaginationRequest.DEFAULT_PAGE_SIZE, result.PageSize);
    }

    [Fact]
    public async Task Handle_WhenPaginationIsSent_ReturnsTheRequestedPage()
    {
        Category technology = FakeBookRepository.CreateCategory(1, "Tecnología");
        FakeBookRepository repository = new FakeBookRepository(
            FakeBookRepository.CreateBook(1, "Arquitectura de software", "9780000000019", 2018, "Autor Uno", technology),
            FakeBookRepository.CreateBook(2, "Fundamentos de bases de datos", "9780000000026", 2016, "Autor Dos", technology),
            FakeBookRepository.CreateBook(3, "Redes distribuidas", "9780000000033", 2020, "Autor Tres", technology));
        GetBooksByCategoryQueryHandler handler = new GetBooksByCategoryQueryHandler(repository, new FakeCategoryRepository(1));
        GetBooksByCategoryQuery query = new(1) { Pagination = new PaginationRequest(pageNumber: 2, pageSize: 2) };

        PaginationResponse<BookDto>? result = await handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(3, result.TotalCount);
        Assert.Equal(2, result.TotalPages);
        Assert.Equal(2, result.PageNumber);
        Assert.True(result.HasPreviousPage);
        Assert.False(result.HasNextPage);
        BookDto book = Assert.Single(result.Items);
        Assert.Equal(3, book.Id);
    }

    [Fact]
    public async Task Handle_WhenCategoryExistsWithoutBooks_ReturnsEmptyPage()
    {
        Category science = FakeBookRepository.CreateCategory(2, "Ciencias");
        FakeBookRepository repository = new FakeBookRepository(
            FakeBookRepository.CreateBook(5, "Historia de la ciencia", "9780000000057", 2012, "Autor Dos", science));
        GetBooksByCategoryQueryHandler handler = new GetBooksByCategoryQueryHandler(repository, new FakeCategoryRepository(1, 2));

        PaginationResponse<BookDto>? result = await handler.Handle(new GetBooksByCategoryQuery(1), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalCount);
        Assert.Equal(0, result.TotalPages);
    }

    [Fact]
    public async Task Handle_WhenCategoryDoesNotExist_ReturnsNull()
    {
        GetBooksByCategoryQueryHandler handler = new GetBooksByCategoryQueryHandler(new FakeBookRepository(), new FakeCategoryRepository(1, 2));

        PaginationResponse<BookDto>? result = await handler.Handle(new GetBooksByCategoryQuery(999), CancellationToken.None);

        Assert.Null(result);
    }
}
