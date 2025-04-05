using ShadowNetBackend.Features.Criminals.Common;

namespace ShadowNetBackend.Features.Criminals.GetAllCriminals;

public record GetCriminalsQuery(CriminalParameters Parameters) : IRequest<IEnumerable<CriminalResponse>>;
