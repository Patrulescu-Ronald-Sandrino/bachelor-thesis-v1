using domain.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bll.Data.Configurations;

public class UserConfiguration : AuditableEntityConfiguration<User>
{
    protected override void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasIndex(user => user.Email).IsUnique();
        builder
            .HasIndex(user => user.Username).IsUnique();
    }
}