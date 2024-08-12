namespace Metalted.BPX.Authentication.Resources;

public class RefreshResource
{
    public ulong SteamId { get; set; }
    public string LoginToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}
