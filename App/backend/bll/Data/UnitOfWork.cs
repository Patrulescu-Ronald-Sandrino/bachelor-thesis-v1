using domain.Repository;
using Microsoft.EntityFrameworkCore.Storage;

namespace bll.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;
    private readonly Dictionary<string, object> _repositories = new();
    private IDbContextTransaction? _transaction;

    public UnitOfWork(DbContext context)
    {
        _context = context;
    }

    public IRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        var typeName = typeof(TEntity).Name;

        lock (_repositories)
        {
            if (_repositories.TryGetValue(typeName, out var existingRepository))
            {
                return (IRepository<TEntity>)existingRepository;
            }

            var repository = new Repository<TEntity>(_context);
            _repositories.Add(typeName, repository);
            return repository;
        }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task StartNewTransactionIfNeeded()
    {
        _transaction ??= await _context.Database.BeginTransactionAsync();
    }

    public async Task BeginTransaction()
    {
        await StartNewTransactionIfNeeded();
    }

    public async Task CommitTransaction()
    {
        /*
         do not open transaction here, because if during the request
         nothing was changed(only select queries were run), we don't
         want to open and commit an empty transaction -calling SaveChanges()
         on _transactionProvider will not send any sql to database in such case
        */
        await _context.SaveChangesAsync();

        if (_transaction == null) return;
        await _transaction.CommitAsync();

        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackTransaction()
    {
        if (_transaction == null) return;

        await _transaction.RollbackAsync();

        await _transaction.DisposeAsync();
        _transaction = null;
    }

    private bool _disposed;

    private void Dispose(bool calledFromDispose)
    {
        // https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
        if (!_disposed)
        {
            // https://stackoverflow.com/questions/538060/proper-use-of-the-idisposable-interface
            if (calledFromDispose)
            {
                _context.Dispose();
            }
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}