namespace ShadowNetBackend.Features.Witnesses.CreateWitness;

public class CreateWitnessCommand : IRequest<Guid?>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Alias { get; set; } = string.Empty;
    public string? Image { get; set; }
    public RiskLevel RiskLevel { get; set; }
    public string? LocationHistory { get; set; }
    public RelocationStatus RelocationStatus { get; set; }
}
