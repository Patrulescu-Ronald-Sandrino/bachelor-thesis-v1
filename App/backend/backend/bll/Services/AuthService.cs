using bll.Services.Helpers;
using domain.Models.Entities;
using domain.Repository;
using domain.Service;
using domain.Service.Models;

namespace bll.Services;

public class AuthService : IAuthService
{
    private readonly IGenericRepository<User> _userRepository;

    private readonly JwtTokenHelper _jwtTokenHelper;

    public AuthService(IGenericRepository<User> userRepository, JwtTokenHelper jwtTokenHelper)
    {
        _userRepository = userRepository;
        _jwtTokenHelper = jwtTokenHelper;
    }

    public async Task<string> Login(LoginCredentials loginCredentials)
    {
        // find account in db by email
        var user = await _userRepository.GetFirst(user => user.Email == loginCredentials.Email);
        if (user == null)
        {
            throw new IAuthService.BadCredentialsException($"User with email {loginCredentials.Email} not found!");
        }

        // validate password
        if (!MatchesHash(loginCredentials.Password, user.PasswordHash))
        {
            throw new IAuthService.BadCredentialsException(
                $"Password for user with email {loginCredentials.Email} is incorrect!");
        }

        // generate token
        return _jwtTokenHelper.GenerateJwtToken(loginCredentials.Email);
    }

    private static bool MatchesHash(string text, string hash) =>
        BCrypt.Net.BCrypt.Verify(text, hash);
    
    public async Task Register(RegisterCredentials registerCredentials)
    {
        // create user
        var user = new User()
        {
            Email = registerCredentials.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerCredentials.Password),
            Username = registerCredentials.Email
        };

        try
        {
            await _userRepository.Insert(user);
        }
        catch (Exception e)
        {
            throw;
        }
    }
}