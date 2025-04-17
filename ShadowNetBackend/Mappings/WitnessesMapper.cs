namespace ShadowNetBackend.Mappings;

public static class WitnessesMapper
{
    public static WitnessDto ToWitnessDto(this Witness witness) => new WitnessDto(
            witness.Id,
            witness.FirstName,
            witness.LastName,
            witness.Alias,
            witness.RiskLevel,
            witness.LocationHistory,
            witness.RelocationStatus
        );

    public static IEnumerable<WitnessDto> ToWitnessDto(this IEnumerable<Witness> witnesses) => witnesses.Select(w => w.ToWitnessDto());

    public static Witness ToWitness(this WitnessForCreationDto witnessDto) => new Witness
    {
        FirstName = witnessDto.FirstName,
        LastName = witnessDto.LastName,
        Alias = witnessDto.Alias ?? string.Empty,
        Image = witnessDto.Image is null
            ? null
            : FileHelper.ConvertFromBase64(witnessDto.Image),
        RiskLevel = witnessDto.RiskLevel,
        LocationHistory = witnessDto.LocationHistory,
        RelocationStatus = witnessDto.RelocationStatus
    };
}