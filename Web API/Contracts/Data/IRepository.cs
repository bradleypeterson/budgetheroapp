using System.Linq.Expressions;

namespace Web_API.Contracts.Data;
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);

    T? Get(Expression<Func<T, bool>> predicate, bool asNotTracking = false, string? includes = null);

    Task<T?> GetAsync(Expression<Func<T, bool>> predicate, bool asNotTracking = false, string? includes = null);

    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);

    IEnumerable<T> List();

    IEnumerable<T> List(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> orderBy = null!, string? includes = null);

    Task<IEnumerable<T?>> ListAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> orderBy = null!, string? includes = null);

    Task<int> AddAsync(T entity);

    Task DeleteAsync(T entity);

    void Delete(IEnumerable<T> entities);

    Task Update(T entity);
}
