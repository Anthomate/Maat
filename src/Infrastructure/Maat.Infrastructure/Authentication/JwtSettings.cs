namespace Maat.Infrastructure.Authentication;

public class JwtSettings
{
    public string Secret { get; init; }
    public int ExpiryMinutes { get; init; }
    public string Issuer { get; init; }
    public string Audience { get; init; }
}