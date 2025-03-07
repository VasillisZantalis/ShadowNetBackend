using ShadowNetBackend.Common;

namespace ShadowNetBackend.Features.Agents;

public class AgentParameters : QueryStringParameters
{
    public string? Name { get; set; }
}
