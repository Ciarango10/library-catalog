using Library.Domain.Repositories;

namespace Library.Tests.Application.Fakes;

internal sealed class FakeCategoryRepository(params int[] existingCategoryIds) : ICategoryRepository
{
    private readonly HashSet<int> _categoryIds = existingCategoryIds.ToHashSet();

    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default) =>
        Task.FromResult(_categoryIds.Contains(id));
}
