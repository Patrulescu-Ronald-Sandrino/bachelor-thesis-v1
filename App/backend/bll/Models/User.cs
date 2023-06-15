using bll.Models.Contracts;

namespace bll.Models;

public class User : AuditableEntity, IAuditableEntity
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }
}