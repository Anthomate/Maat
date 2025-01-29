namespace Maat.Application.Common.Models;

public record AuthenticationResult
{
    public bool Success { get; init; }
    public string Token { get; init; }
    public string[] Errors { get; init; }

    private AuthenticationResult(bool success, string token, string[] errors)
    {
        Success = success;
        Token = token;
        Errors = errors;
    }

    public static AuthenticationResult Succeeded(string token) =>
        new(true, token, Array.Empty<string>());

    public static AuthenticationResult Failed(IEnumerable<string> errors) =>
        new(false, string.Empty, errors.ToArray());
}