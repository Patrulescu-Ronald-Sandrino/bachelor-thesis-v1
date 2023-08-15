using System.Linq.Expressions;

namespace domain.Repository;

public interface IRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "", bool tracking = false);

    Task<TEntity?> GetFirst(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "", bool tracking = false);

    Task<TEntity?> GetById(object id);

    Task Insert(TEntity entity, bool saveChanges = true);

    Task Update(TEntity entityToUpdate, bool saveChanges = true);

    Task Delete(object id, bool saveChanges = true);

    Task Delete(TEntity entityToDelete, bool saveChanges = true);
}