using Library.Application.Utilities.Pagination;

namespace Library.Tests.Application.Pagination;

public class PaginationResponseTests
{
    [Theory]
    [InlineData(0, 10, 0)]
    [InlineData(10, 10, 1)]
    [InlineData(11, 10, 2)]
    [InlineData(32, 10, 4)]
    public void TotalPages_RoundsUpTheNumberOfPages(int totalCount, int pageSize, int expectedTotalPages)
    {
        PaginationResponse<int> response = PaginationResponse<int>.Create([], totalCount, new PaginationRequest(1, pageSize));

        Assert.Equal(expectedTotalPages, response.TotalPages);
    }

    [Theory]
    [InlineData(1, false, true)]
    [InlineData(2, true, true)]
    [InlineData(4, true, false)]
    public void Navigation_IndicatesIfThereArePreviousAndNextPages(
        int pageNumber, bool expectedHasPreviousPage, bool expectedHasNextPage)
    {
        PaginationResponse<int> response = PaginationResponse<int>.Create([], 32, new PaginationRequest(pageNumber, 10));

        Assert.Equal(expectedHasPreviousPage, response.HasPreviousPage);
        Assert.Equal(expectedHasNextPage, response.HasNextPage);
    }
}
