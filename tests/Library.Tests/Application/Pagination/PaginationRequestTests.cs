using Library.Application.Utilities.Pagination;

namespace Library.Tests.Application.Pagination;

public class PaginationRequestTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Constructor_WhenPageNumberIsLessThanOne_UsesTheFirstPage(int pageNumber)
    {
        PaginationRequest request = new PaginationRequest(pageNumber, 10);

        Assert.Equal(1, request.PageNumber);
    }

    [Theory]
    [InlineData(0, PaginationRequest.DEFAULT_PAGE_SIZE)]
    [InlineData(-1, PaginationRequest.DEFAULT_PAGE_SIZE)]
    [InlineData(1000, PaginationRequest.MAX_PAGE_SIZE)]
    public void Constructor_WhenPageSizeIsOutOfRange_AdjustsIt(int pageSize, int expectedPageSize)
    {
        PaginationRequest request = new PaginationRequest(1, pageSize);

        Assert.Equal(expectedPageSize, request.PageSize);
    }
}
