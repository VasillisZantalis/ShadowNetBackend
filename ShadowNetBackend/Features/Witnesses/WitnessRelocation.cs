namespace ShadowNetBackend.Features.Witnesses;

public class WitnessRelocation
{
    public int Id { get; set; }
    public Guid WitnessId { get; set; }
    public Witness? Witness { get; set; }
    public int SafeHouseId { get; set; }
    public SafeHouse? SafeHouse { get; set; }
    public DateTime RelocationDate { get; set; }

}
