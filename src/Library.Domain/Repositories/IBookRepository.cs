using Library.Domain.Entities;

namespace Library.Domain.Repositories;

public interface IBookRepository
{
    Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene una página de los libros de una categoría y el total de libros de esa categoría.
    /// </summary>
    Task<(IReadOnlyList<Book> Items, int TotalCount)> GetByCategoryAsync(
        int categoryId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}
