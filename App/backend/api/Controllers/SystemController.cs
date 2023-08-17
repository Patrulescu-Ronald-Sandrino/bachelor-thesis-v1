using api.Controllers.Utils;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

public class SystemController : ApiController
{
    [ActionName("health-check")]
    [HttpGet]
    public IActionResult HealthCheck()
    {
        return Ok();
    }
}