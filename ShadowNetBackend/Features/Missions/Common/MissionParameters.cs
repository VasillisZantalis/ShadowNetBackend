using ShadowNetBackend.Common;

namespace ShadowNetBackend.Features.Missions.Common;

public class MissionParameters : QueryStringParameters
{
    public MissionStatus Status { get; set; }
    public RiskLevel Risk { get; set; }
}
