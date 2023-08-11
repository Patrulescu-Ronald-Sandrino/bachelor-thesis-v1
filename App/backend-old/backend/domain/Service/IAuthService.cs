using domain.Service.Models;

namespace domain.Service;

public interface IAuthService
{
    string Login(LoginCredentials loginCredentials);

    void Register(RegisterCredentials registerCredentials);

    class BadCredentialsException : Exception
    {
        public BadCredentialsException(string? message) : base(message)
        {
        }
    }
}