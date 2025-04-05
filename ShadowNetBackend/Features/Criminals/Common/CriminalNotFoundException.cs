using ShadowNetBackend.Exceptions;

namespace ShadowNetBackend.Features.Criminals.Common;

public class CriminalNotFoundException : NotFoundException
{
    public CriminalNotFoundException(string message = "Criminal was not found")
        : base(message) { }
}
