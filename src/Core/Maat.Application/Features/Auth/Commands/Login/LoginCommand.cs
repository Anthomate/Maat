using Maat.Application.Common.Models;
using MediatR;

namespace Maat.Application.Features.Auth.Commands.Login;

public record LoginCommand : IRequest<AuthenticationResult>
{
    public string Email { get; init; }
    public string Password { get; init; }
}