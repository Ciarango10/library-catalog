using Library.Domain.Entities;

namespace Library.Domain.Repositories;

public interface IBookRepository
{
    Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Book>> GetByCategoryAsync(int categoryId, CancellationToken cancellationToken = default);
}
