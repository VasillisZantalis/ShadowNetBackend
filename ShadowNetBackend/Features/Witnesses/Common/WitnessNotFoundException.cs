using ShadowNetBackend.Exceptions;

namespace ShadowNetBackend.Features.Witnesses.Common;

public class WitnessNotFoundException : NotFoundException
{
    public WitnessNotFoundException(string message = "Witness not found")
        : base(message) { }
}
