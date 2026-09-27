using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Extensions;

internal static class QueryableExtensions
{
    /// <summary>
    /// Ejecuta la consulta paginada: cuenta el total de registros y obtiene
    /// únicamente los de la página solicitada. La consulta debe llegar ordenada.
    /// </summary>
    public static async Task<(IReadOnlyList<T> Items, int TotalCount)> ToPagedListAsync<T>(
        this IQueryable<T> query,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(pageNumber, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);

        int totalCount = await query.CountAsync(cancellationToken);

        List<T> items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
