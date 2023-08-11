namespace domain.Models.AuditableEntity;

public abstract class AuditableEntity : IAuditableEntity
{
    public int Id { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateModified { get; set; }

    public int Version { get; set; }
}