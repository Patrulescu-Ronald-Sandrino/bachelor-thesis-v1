using domain.Service.Models;

namespace domain.Service;

public interface IAuthService
{
    Task<string> Login(LoginCredentials loginCredentials);

    Task Register(RegisterCredentials registerCredentials);

    class BadCredentialsException : Exception
    {
        public BadCredentialsException(string? message) : base(message)
        {
        }
    }
}