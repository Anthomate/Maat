using MediatR;
using Microsoft.AspNetCore.Mvc;
using Maat.Application.Features.Auth.Commands.Register;
using Maat.Application.Features.Auth.Commands.Login;

namespace Maat.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(new { Errors = result.Errors });
        }

        return Ok(new { Token = result.Token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return Unauthorized(new { Errors = result.Errors });
        }

        return Ok(new { Token = result.Token });
    }
}