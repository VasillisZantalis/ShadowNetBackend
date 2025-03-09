using ShadowNetBackend.Features.Witnesses;
using ShadowNetBackend.Features.Witnesses.Common;

namespace ShadowNetBackend.Mappings;

public static class WitnessesMapper
{
    public static WitnessResponse ToWitnessResponse(this Witness witness)
    {
        return new WitnessResponse
        (
            witness.Id,
            witness.FirstName,
            witness.LastName,
            witness.Alias,
            witness.RiskLevel,
            witness.LocationHistory,
            witness.RelocationStatus
        );
    }
}
