using Library.Domain.Repositories;
using Library.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories;

public sealed class CategoryRepository(LibraryDbContext context) : ICategoryRepository
{
    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default) =>
        await context.Categories.AnyAsync(category => category.Id == id, cancellationToken);
}
