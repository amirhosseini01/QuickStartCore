using System.Linq.Expressions;
using Server.Core.Data.Models;

namespace Server.Core.Data.Repositories.Contracts;

public interface IGenericRepository<T>
{
    Task AddAsync(T item, CancellationToken ct = default);
    Task AddRangeAsync(ICollection<T> items, CancellationToken ct = default);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, BaseQueryFilter? filterQuery = null, CancellationToken ct = default);
    Task<T?> FindAsync(object id, CancellationToken ct = default);
    void Remove(T item);
    void RemoveRange(ICollection<T> items);
    Task SaveChangesAsync(CancellationToken ct = default);
    void UpdateRange(ICollection<T> items);
}
