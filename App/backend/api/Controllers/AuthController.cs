using api.Controllers.Utils;
using domain.Service;
using domain.Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

public class AuthController : ApiController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public async Task<ActionResult<string>> Login([FromBody] LoginCredentials loginCredentials)
    {
        return await _authService.Login(loginCredentials);
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterCredentials registerCredentials)
    {
        await _authService.Register(registerCredentials);
        return Ok();
    }
}