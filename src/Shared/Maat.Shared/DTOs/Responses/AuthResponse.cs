namespace Maat.Shared.DTOs.Responses;

public class AuthResponse
{
    public bool Success { get; set; }
    public string Token { get; set; }
    public List<string> Errors { get; set; }

    public AuthResponse()
    {
        Token = string.Empty;
        Errors = new List<string>();
    }
}