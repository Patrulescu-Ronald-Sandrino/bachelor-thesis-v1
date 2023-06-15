using api.Utils;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[RouteApiController]
public class TestController : ControllerBase
{

    [HttpGet(Name = "get")]
    public void Get(string email)
    {
        
    }
}