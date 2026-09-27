namespace Library.Domain.Repositories;

public interface ICategoryRepository
{
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
