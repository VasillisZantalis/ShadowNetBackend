using MediatR;
using ShadowNetBackend.Features.Witnesses.Common;

namespace ShadowNetBackend.Features.Witnesses.GetAllWitnesses;

public record GetWitnessesQuery(WitnessParameters Parameters) : IRequest<IEnumerable<WitnessResponse>>;
