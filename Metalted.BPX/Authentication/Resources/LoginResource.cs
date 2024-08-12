namespace Metalted.BPX.Authentication.Resources;

public class LoginResource
{
    public ulong SteamId { get; set; }
    public string SteamName { get; set; } = null!;
    public string AuthenticationTicket { get; set; } = null!;
}
