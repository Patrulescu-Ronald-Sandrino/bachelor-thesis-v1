using bll.Services.Helpers;
using domain.Exceptions;
using domain.Models.Entities;
using domain.Repository;
using domain.Service;
using domain.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace bll.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly JwtTokenHelper _jwtTokenHelper;

    public AuthService(IUnitOfWork unitOfWork, JwtTokenHelper jwtTokenHelper)
    {
        _jwtTokenHelper = jwtTokenHelper;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Login(AuthCredentials authCredentials)
    {
        // find account in db by email
        var user = await _unitOfWork.Repository<User>().GetFirst(user => user.Email == authCredentials.Email);
        if (user == null)
        {
            throw new NotFound("User not found!");
        }

        // validate password
        if (!MatchesHash(authCredentials.Password, user.PasswordHash))
        {
            throw new Unauthorized("Password is incorrect!");
        }

        // generate token
        return _jwtTokenHelper.GenerateJwtToken(authCredentials.Email);
    }

    private static bool MatchesHash(string text, string hash) => BCrypt.Net.BCrypt.Verify(text, hash);

    public async Task Register(AuthCredentials authCredentials)
    {
        // create user
        var user = new User
        {
            Email = authCredentials.Email,
            PasswordHash = HashPassword(authCredentials.Password),
            Username = Guid.NewGuid().ToString()
        };

        try
        {
            await _unitOfWork.Repository<User>().Insert(user);
        }
        catch (DbUpdateException e)
        {
            if ($"Cannot insert duplicate key row in object 'dbo.{nameof(User)}' with unique index 'IX_{nameof(User)}_Email'. The duplicate key value is ({authCredentials.Email})."
                .Equals(e.InnerException?.Message))
            {
                throw new Conflict($"Email is already used!");
            }

            throw;
        }
    }

    private static string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);
}