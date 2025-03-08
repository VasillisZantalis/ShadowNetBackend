using ShadowNetBackend.Exceptions;

namespace ShadowNetBackend.Features.Missions.Common;

public class MissionNotFoundException : NotFoundException
{
    public MissionNotFoundException(string message = "Mission was not found")
        : base(message) { }
}
