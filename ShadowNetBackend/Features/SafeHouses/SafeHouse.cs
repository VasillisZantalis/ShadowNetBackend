using ShadowNetBackend.Features.Witnesses;

namespace ShadowNetBackend.Features.SafeHouses;

public class SafeHouse
{
    public int Id { get; set; }
    public string Location { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public bool IsActive { get; set; }
    public ICollection<WitnessRelocation> WitnessRelocations { get; set; } = new List<WitnessRelocation>();
}
