using Maat.Domain.Entities;

namespace Maat.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}