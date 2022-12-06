using System.Linq.Expressions;

namespace DesktopApplication.Contracts.Data;
public interface IRepository<T> where T : class
{
    T? GetById(Guid id);

    T? Get(Expression<Func<T, bool>> predicate, bool asNotTracking = false, string? includes = null);

    Task<T?> GetAsync(Expression<Func<T, bool>> predicate, bool asNotTracking = false, string? includes = null);

    IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);

    IEnumerable<T> List();

    IEnumerable<T> List(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> orderBy = null!, string? includes = null);

    Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> orderBy = null!, string? includes = null);

    Task<int> AddAsync(T entity);

    Task<int> AddAsync(IEnumerable<T> entities);

    Task DeleteAsync(T entity);

    void Delete(IEnumerable<T> entities);

    Task Update(T entity);
}
