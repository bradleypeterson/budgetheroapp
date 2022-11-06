using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApplication.Contracts.Data;
public interface IRepository<T> where T : class
{
    T? GetById(int id);

    T? Get(Expression<Func<T, bool>> predicate, bool asNotTracking = false, string? includes = null);

    Task<T?> GetAsync(Expression<Func<T, bool>> predicate, bool asNotTracking = false, string? includes = null);

    IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);

    IEnumerable<T> List();

    IEnumerable<T> List(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> orderBy = null!, string? includes = null);

    Task<IEnumerable<T?>> ListAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> orderBy = null!, string? includes = null);

    void Add(T entity);

    void Delete(T entity);

    void Delete(IEnumerable<T> entities);

    void Update(T entity);
}
