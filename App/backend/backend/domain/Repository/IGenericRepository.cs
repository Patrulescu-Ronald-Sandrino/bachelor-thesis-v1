using System.Linq.Expressions;

namespace domain.Repository;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "");

    Task<TEntity?> GetFirst(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "");
    
    Task<TEntity?> GetById(object id);
    
    Task Insert(TEntity entity);
    
    Task Delete(object id);
    
    void Delete(TEntity entityToDelete);
    
    Task Update(TEntity entityToUpdate);
}