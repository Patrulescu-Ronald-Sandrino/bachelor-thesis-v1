namespace bll.Models.Contracts;

public interface IAuditableEntity // TODO: try w/o setters
// TODO: try another approach: https://stackoverflow.com/questions/46978332/use-ientitytypeconfiguration-with-a-base-entity
{
    public int Id { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateModified { get; set; }

    public int Version { get; set; }
}