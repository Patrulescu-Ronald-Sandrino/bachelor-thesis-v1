using api.Controllers.Utils;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

public class TestController : ApiController // TODO: hide/lock it in production
{
    [HttpGet]
    public string Get(string email)
    {
        return email;
    }
}