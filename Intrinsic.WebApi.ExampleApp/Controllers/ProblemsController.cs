using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Intrinsic.WebApi.ExampleApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProblemsController : ControllerBase
{
    [HttpGet("throwError")]
    public IActionResult ThrowError()
    {
        throw new Exception("Some problem has happened!");
    }
}
