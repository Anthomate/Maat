using Maat.Application.Common.Interfaces;
using Maat.Application.Common.Models;
using Maat.Domain.Entities;
using Maat.Domain.Interfaces;
using Maat.Domain.Interfaces.Repositories;
using MediatR;

namespace Maat.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthenticationResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (await _userRepository.ExistsByEmailAsync(request.Email))
            {
                return AuthenticationResult.Failed(new[] { "Un utilisateur avec cet email existe déjà." });
            }

            var user = new User(
                request.Email,
                _passwordHasher.HashPassword(request.Password),
                request.FirstName,
                request.LastName);

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var token = _jwtTokenGenerator.GenerateToken(user);

            return AuthenticationResult.Succeeded(token);
        }
        catch (Exception ex)
        {
            return AuthenticationResult.Failed(new[] { "Une erreur est survenue lors de l'inscription." });
        }
    }
}