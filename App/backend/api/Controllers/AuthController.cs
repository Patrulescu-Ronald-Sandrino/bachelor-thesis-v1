using api.Controllers.Utils;
using api.DTOs;
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse)),
     ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse)),
     ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] AuthCredentials authCredentials)
    {
        return Ok(new LoginResponse { Token = await _authService.Login(authCredentials) });
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(void)),
     ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> Register([FromBody] AuthCredentials authCredentials)
    {
        await _authService.Register(authCredentials);
        return NoContent();
    }
}