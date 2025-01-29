using Maat.Application.Common.Models;
using MediatR;

namespace Maat.Application.Features.Auth.Commands.Register;

public record RegisterCommand : IRequest<AuthenticationResult>
{
    public string Email { get; init; }
    public string Password { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
}