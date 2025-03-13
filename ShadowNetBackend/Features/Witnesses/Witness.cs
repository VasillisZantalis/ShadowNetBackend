namespace ShadowNetBackend.Features.Witnesses;

public class Witness
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Alias { get; set; } = string.Empty;
    public byte[]? Image { get; set; }
    public RiskLevel RiskLevel { get; set; }
    public string? LocationHistory { get; set; }
    public RelocationStatus RelocationStatus { get; set; } = RelocationStatus.None;
    public ICollection<WitnessRelocation> Relocations { get; set; } = new List<WitnessRelocation>();
}
