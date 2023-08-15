using System.Data;
using domain.Models.AuditableEntity;
using domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace bll.Data;

public class DbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbContext(DbContextOptions<DbContext> options) : base(options)
    {
        ChangeTracker.StateChanged += UpdateIEntity;
        ChangeTracker.Tracked += UpdateIEntity;
    }

    #region DbSets

    public virtual DbSet<User> Users { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DbContext).Assembly);
    }

    private static void UpdateIEntity(object? sender, EntityEntryEventArgs e)
    {
        if (e.Entry.Entity is IAuditableEntity entity)
        {
            switch (e.Entry.State)
            {
                case EntityState.Added:
                    entity.DateCreated = DateTime.UtcNow;
                    entity.DateModified = DateTime.UtcNow;
                    entity.Version = 0;
                    break;
                case EntityState.Modified:
                    entity.DateModified = DateTime.UtcNow;
                    entity.Version++;
                    break;
            }
        }
    }
}