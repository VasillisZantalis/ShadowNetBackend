using ShadowNetBackend.Exceptions;

namespace ShadowNetBackend.Features.Agents.Common;

public class AgentNotFoundException : NotFoundException
{
    public AgentNotFoundException(string message = "Agent was not found")
        : base (message) { }
}
