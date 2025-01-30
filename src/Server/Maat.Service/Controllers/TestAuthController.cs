using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Maat.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestAuthController : ControllerBase
{
    [HttpGet]
    [Authorize]
    [Route("protected")]
    public IActionResult Protected()
    {
        return Ok(new { message = "This is a protected endpoint" });
    }

    [HttpGet]
    [Route("public")]
    public IActionResult Public()
    {
        return Ok(new { message = "This is a public endpoint" });
    }
}