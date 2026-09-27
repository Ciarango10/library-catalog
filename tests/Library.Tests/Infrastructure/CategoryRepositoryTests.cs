using Library.Infrastructure.Repositories;

namespace Library.Tests.Infrastructure;

public sealed class CategoryRepositoryTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task ExistsAsync_WhenCategoryIsSeeded_ReturnsTrue(int categoryId)
    {
        await using TestDatabase database = await TestDatabase.CreateAsync();
        CategoryRepository repository = new CategoryRepository(database.Context);

        bool exists = await repository.ExistsAsync(categoryId);

        Assert.True(exists);
        Assert.Empty(database.Context.ChangeTracker.Entries());
    }

    [Fact]
    public async Task ExistsAsync_WhenCategoryDoesNotExist_ReturnsFalse()
    {
        await using TestDatabase database = await TestDatabase.CreateAsync();
        CategoryRepository repository = new CategoryRepository(database.Context);

        bool exists = await repository.ExistsAsync(999);

        Assert.False(exists);
    }
}
