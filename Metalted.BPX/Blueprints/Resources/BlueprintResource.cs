namespace Metalted.BPX.Blueprints.Resources;

public class BlueprintResource
{
    public string Name { get; set; } = null!;
    public List<string> Tags { get; set; } = null!;
    public string BlueprintBase64 { get; set; } = null!;
    public string ImageBase64 { get; set; } = null!;
}
