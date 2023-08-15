namespace domain.Repository;

public interface IUnitOfWork : IDisposable
{
    IRepository<TEntity> Repository<TEntity>() where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task BeginTransaction();

    Task CommitTransaction();

    Task RollbackTransaction();
}