using domain.Models.AuditableEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bll.Data.Configurations;

public abstract class AuditableEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : AuditableEntity
{
    void IEntityTypeConfiguration<T>.Configure(EntityTypeBuilder<T> builder)
    {
        builder
            .HasKey(auditableEntity => auditableEntity.Id);

        Configure(builder);
    }

    protected abstract void Configure(EntityTypeBuilder<T> builder);
}