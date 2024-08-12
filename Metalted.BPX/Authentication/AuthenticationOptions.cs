namespace Metalted.BPX.Authentication;

public class AuthenticationOptions
{
    public const string Key = "Authentication";

    public string JwtKey { get; set; } = string.Empty;
}
