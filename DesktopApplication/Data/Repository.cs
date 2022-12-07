using System.Diagnostics;
using System.Linq.Expressions;
using DesktopApplication.Contracts.Data;
using Microsoft.EntityFrameworkCore;

namespace DesktopApplication.Data;
public class Repository<T> : IRepository<T> where T : class
{
    protected BudgetAppContext _context;

    internal DbSet<T> dbset;
    public Repository(BudgetAppContext context)
    {
        _context = context;

        dbset = _context.Set<T>();
    }

    public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
    {
        IQueryable<T> query = dbset;
        if (filter != null)
        {
            query = query.Where(filter);
        }
        if (includeProperties != null)
        {
            foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }
        return query.ToList();
    }

    public async virtual Task<int> AddAsync(T entity)
    {
        _context.Set<T>().Add(entity);
        return await _context.SaveChangesAsync();
    }

    public async virtual Task<int> AddAsync(IEnumerable<T> entities)
    {
        _context.Set<T>().AddRange(entities);
        return await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public void Delete(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
        _context.SaveChanges();
    }

    public virtual T? Get(Expression<Func<T, bool>> predicate, bool asNotTracking = false, string? includes = null)
    {
        if (includes == null)
        {
            if (asNotTracking)
            {
                return _context.Set<T>()
                                 .AsNoTracking()
                                 .Where(predicate)
                                 .FirstOrDefault();
            }
            else
            {
                return _context.Set<T>()
                    .Where(predicate)
                    .FirstOrDefault();
            }
        }
        else
        {
            IQueryable<T> queryable = _context.Set<T>();
            foreach (var includeProperty in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                queryable = queryable.Include(includeProperty);
            }

            if (asNotTracking)
            {
                return queryable
                    .AsNoTracking()
                    .Where(predicate)
                    .FirstOrDefault();
            }
            else
            {
                return queryable
                    .Where(predicate)
                    .FirstOrDefault();
            }
        }
    }

    public async virtual Task<T?> GetAsync(Expression<Func<T, bool>> predicate, bool asNotTracking = false, string? includes = null)
    {
        if (includes == null)
        {
            if (asNotTracking)
            {
                return await _context.Set<T>()
                    .AsNoTracking()
                    .Where(predicate)
                    .FirstOrDefaultAsync();
            }
            else
            {
                return await _context.Set<T>()
                    .Where(predicate)
                    .FirstOrDefaultAsync();
            }
        }
        else
        {
            IQueryable<T> queryable = _context.Set<T>();
            foreach (var includeProperty in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                queryable = queryable.Include(includeProperty);
            }

            if (asNotTracking)
            {
                return await queryable
                    .AsNoTracking()
                    .Where(predicate)
                    .FirstOrDefaultAsync();
            }
            else
            {
                return await queryable
                    .Where(predicate)
                    .FirstOrDefaultAsync();
            }
        }
    }

    public virtual T? GetById(Guid id)
    {
        return _context.Set<T>().Find(id);
    }

    public virtual IEnumerable<T> List()
    {
        return _context.Set<T>().ToList().AsEnumerable();
    }

    public virtual IEnumerable<T> List(Expression<Func<T, bool>> predicate, Expression<Func<T, int>>? orderBy = null, string? includes = null)
    {
        IQueryable<T> queryable = _context.Set<T>();
        if (predicate != null && includes == null)
        {
            return _context.Set<T>()
                .Where(predicate)
                .AsEnumerable();
        }
        // have includes
        else if (includes != null)
        {
            foreach (var includeProperty in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                queryable = queryable.Include(includeProperty);
            }
        }

        if (predicate == null)
        {
            if (orderBy == null)
            {
                return queryable.AsEnumerable();
            }
            else
            {
                return queryable.OrderBy(orderBy).ToList().AsEnumerable();
            }
        }
        else
        {
            if (orderBy == null)
            {

                return queryable.ToList().AsEnumerable();

            }
            else
            {
                return queryable.Where(predicate).OrderBy(orderBy).ToList().AsEnumerable();
            }
        }
    }

    public async virtual Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>>? orderBy = null, string? includes = null)
    {
        IQueryable<T> queryable = _context.Set<T>();
        if (predicate != null && includes == null)
        {
            return _context.Set<T>()
                .Where(predicate)
                .AsEnumerable();
        }
        // have includes
        else if (includes != null)
        {
            foreach (var includeProperty in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                queryable = queryable.Include(includeProperty);
            }
        }

        if (predicate == null)
        {
            if (orderBy == null)
            {
                return queryable.AsEnumerable();
            }
            else
            {
                return await queryable.OrderBy(orderBy).ToListAsync();
            }
        }
        else
        {
            if (orderBy == null)
            {

                return await queryable.ToListAsync();

            }
            else
            {
                return await queryable.Where(predicate).OrderBy(orderBy).ToListAsync();
            }
        }
    }

    public async Task Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
}