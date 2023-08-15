using domain.Models.AuditableEntity;
using domain.Models.Enums;

namespace domain.Models.Entities;

public class User : AuditableEntity.AuditableEntity, IAuditableEntity
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Username { get; set; }
    public UserType UserType { get; set; }
}