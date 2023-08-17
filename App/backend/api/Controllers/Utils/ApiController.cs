using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers.Utils;

[Route("api/[controller]/[action]")]
// #pragma warning disable CA1825
[Produces(MediaTypeNames.Application.Json)]
// #pragma warning restore CA1825
public class ApiController : ControllerBase
{
}