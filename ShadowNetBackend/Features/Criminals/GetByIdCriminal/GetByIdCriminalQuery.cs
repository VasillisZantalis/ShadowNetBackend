using ShadowNetBackend.Features.Criminals.Common;

namespace ShadowNetBackend.Features.Criminals.GetByIdCriminal;

public record GetByIdCriminalQuery(Guid Id) : IRequest<CriminalResponse>;
