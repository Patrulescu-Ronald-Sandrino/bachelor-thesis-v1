using System.Linq.Expressions;
using domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace bll.Data;

static class Functional
{
    public static T Identity<T>(T x) => x;
}

// source: https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly DbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "", bool tracking = false)
    {
        IQueryable<TEntity> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter).SetTracking(tracking);
        }

        foreach (var includeProperty in includeProperties.Split
                     (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty).SetTracking(tracking);
        }

        return await (orderBy != null ? orderBy(query) : query).SetTracking(tracking).ToListAsync();
    }

    public async Task<TEntity?> GetFirst(Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "",
        bool tracking = false)
    {
        return (await Get(filter, orderBy, includeProperties, tracking)).FirstOrDefault();
    }

    public virtual async Task<TEntity?> GetById(object id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task Insert(TEntity entity, bool saveChanges = true)
    {
        await _dbSet.AddAsync(entity);

        await SaveChanges(saveChanges);
    }

    public virtual async Task Update(TEntity entityToUpdate, bool saveChanges = true)
    {
        _dbSet.Attach(entityToUpdate);
        _context.Entry(entityToUpdate).State = EntityState.Modified;

        await SaveChanges(saveChanges);
    }

    public virtual async Task Delete(object id, bool saveChanges = true)
    {
        var entityToDelete = await GetById(id);
        await Delete(entityToDelete, saveChanges);
    }

    public virtual async Task Delete(TEntity entityToDelete, bool saveChanges = true)
    {
        if (_context.Entry(entityToDelete).State == EntityState.Detached)
        {
            _dbSet.Attach(entityToDelete);
        }

        _dbSet.Remove(entityToDelete);

        await SaveChanges(saveChanges);
    }

    private async Task SaveChanges(bool saveChanges)
    {
        if (saveChanges)
        {
            await _context.SaveChangesAsync();
        }
    }
}