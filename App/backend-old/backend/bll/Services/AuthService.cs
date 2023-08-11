using domain.Service;
using domain.Service.Models;

namespace bll.Services;

public class AuthService : IAuthService
{
    public string Login(LoginCredentials loginCredentials)
    {
        // find account in db by email
        // validate password
        // generate token
        throw new NotImplementedException();
    }
    
    private static bool MatchesHash(string text, string hash) =>
        BCrypt.Net.BCrypt.Verify(text, hash);

    public void Register(RegisterCredentials registerCredentials)
    {
        throw new NotImplementedException();
    }
}