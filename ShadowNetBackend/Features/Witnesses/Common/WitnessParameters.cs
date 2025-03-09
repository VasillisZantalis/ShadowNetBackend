using ShadowNetBackend.Common;

namespace ShadowNetBackend.Features.Witnesses.Common;

public class WitnessParameters : QueryStringParameters
{
    public string? Name { get; set; }
    public string? Alias { get; set; }
}
