using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DesktopApplication.Contracts.Data;
using Microsoft.EntityFrameworkCore;

namespace DesktopApplication.Data;
public class Repository<T> : IRepository<T> where T : class
{
    private readonly BudgetAppContext dbContext;

    internal DbSet<T> dbset;
    public Repository(BudgetAppContext context)
    {
        dbContext = context;

        dbset = dbContext.Set<T>();
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

    public void Add(T entity)
    {
        dbContext.Set<T>().Add(entity);
        dbContext.SaveChanges();
    }

    public void Delete(T entity)
    {
        dbContext.Set<T>().Remove(entity);
        dbContext.SaveChanges();
    }

    public void Delete(IEnumerable<T> entities)
    {
        dbContext.Set<T>().RemoveRange(entities);
        dbContext.SaveChanges();
    }

    public virtual T? Get(Expression<Func<T, bool>> predicate, bool asNotTracking = false, string? includes = null)
    {
        if (includes == null)
        {
            if (asNotTracking)
            {
                return dbContext.Set<T>()
                                 .AsNoTracking()
                                 .Where(predicate)
                                 .FirstOrDefault();
            }
            else
            {
                return dbContext.Set<T>()
                    .Where(predicate)
                    .FirstOrDefault();
            }
        }
        else
        {
            IQueryable<T> queryable = dbContext.Set<T>();
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
                return await dbContext.Set<T>()
                    .AsNoTracking()
                    .Where(predicate)
                    .FirstOrDefaultAsync();
            }
            else
            {
                return await dbContext.Set<T>()
                    .Where(predicate)
                    .FirstOrDefaultAsync();
            }
        }
        else
        {
            IQueryable<T> queryable = dbContext.Set<T>();
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

    public virtual T? GetById(int id)
    {
        return dbContext.Set<T>().Find(id);
    }

    public virtual IEnumerable<T> List()
    {
        return dbContext.Set<T>().ToList().AsEnumerable();
    }

    public virtual IEnumerable<T> List(Expression<Func<T, bool>> predicate, Expression<Func<T, int>>? orderBy = null, string? includes = null)
    {
        IQueryable<T> queryable = dbContext.Set<T>();
        if (predicate != null && includes == null)
        {
            return dbContext.Set<T>()
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

    public async virtual Task<IEnumerable<T?>> ListAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>>? orderBy = null, string? includes = null)
    {
        IQueryable<T> queryable = dbContext.Set<T>();
        if (predicate != null && includes == null)
        {
            return dbContext.Set<T>()
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

    public void Update(T entity)
    {
        dbContext.Entry(entity).State = EntityState.Modified;
        dbContext.SaveChanges();
    }
}
