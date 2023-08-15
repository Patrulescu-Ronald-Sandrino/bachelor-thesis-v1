using bll.Services;
using domain.Service;

namespace UnitTests.Service;

public class AuthServiceTest
{
    private readonly IAuthService _authService = new AuthService();
    
    [Fact]
    public void WhenLoginWithInvalidEmail_ThenThrowBadCredentials()
    {
        Assert.True(false);
    }

    [Fact]
    public void WhenLoginWithInvalidPassword_ThenThrowBadCredentials()
    {
        Assert.True(false);
    }

    [Fact]
    public void WhenLoginWithValidCredentials_ThenTokenIsValid()
    {
        Assert.True(false);
    }
}