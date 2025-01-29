using Maat.Application.Common.Interfaces;
using Maat.Application.Common.Models;
using Maat.Domain.Interfaces.Repositories;
using MediatR;

namespace Maat.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthenticationResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null)
            {
                return AuthenticationResult.Failed(new[] { "Identifiants invalides." });
            }

            if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                return AuthenticationResult.Failed(new[] { "Identifiants invalides." });
            }

            var token = _jwtTokenGenerator.GenerateToken(user);

            return AuthenticationResult.Succeeded(token);
        }
        catch (Exception ex)
        {
            return AuthenticationResult.Failed(new[] { "Une erreur est survenue lors de la connexion." });
        }
    }
}