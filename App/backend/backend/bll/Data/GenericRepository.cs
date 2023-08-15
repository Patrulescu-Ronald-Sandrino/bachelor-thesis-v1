using System.Linq.Expressions;
using domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace bll.Data;

// source: https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    internal DbContext context;
    internal DbSet<TEntity> dbSet;

    public GenericRepository(DbContext context)
    {
        this.context = context;
        dbSet = context.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "")
    {
        IQueryable<TEntity> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split
                     (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }
        
        return await query.ToListAsync();
    }

    public async Task<TEntity?> GetFirst(Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "")
    {
        return (await Get(filter, orderBy, includeProperties)).First();
    }

    public virtual async Task<TEntity?> GetById(object id)
    {
        return await dbSet.FindAsync(id);
    }

    public virtual async Task Insert(TEntity entity)
    {
        await dbSet.AddAsync(entity);
    }

    public virtual async Task Delete(object id)
    {
        var entityToDelete = await dbSet.FindAsync(id);
        Delete(entityToDelete);
    }

    public virtual void Delete(TEntity entityToDelete)
    {
        if (context.Entry(entityToDelete).State == EntityState.Detached)
        {
            dbSet.Attach(entityToDelete);
        }

        dbSet.Remove(entityToDelete);
    }

    public virtual async Task Update(TEntity entityToUpdate)
    {
        dbSet.Attach(entityToUpdate);
        context.Entry(entityToUpdate).State = EntityState.Modified;
    }
}