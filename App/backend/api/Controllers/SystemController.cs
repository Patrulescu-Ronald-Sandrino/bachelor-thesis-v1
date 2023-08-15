using api.Controllers.Utils;
using api.Utils;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[RouteApiController]
public class SystemController : ApiController
{
    [HttpGet("/health-check")]
    public IActionResult HealthCheck()
    {
        return Ok();
    }
}