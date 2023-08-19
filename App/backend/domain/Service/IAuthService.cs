using domain.Service.Models;

namespace domain.Service;

public interface IAuthService
{
    Task<string> Login(AuthCredentials authCredentials);

    Task Register(AuthCredentials authCredentials);
}