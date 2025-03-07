using ShadowNetBackend.Common;

namespace ShadowNetBackend.Features.Witnesses;

public class Witness
{
    public Guid Id { get; set; }
    public string Alias { get; set; } = string.Empty;
    public RiskLevel RiskLevel { get; set; }
    public string? LocationHistory { get; set; }
    public RelocationStatus RelocationStatus { get; set; }

    public ICollection<WitnessRelocation> Relocations { get; set; } = new List<WitnessRelocation>();
}
