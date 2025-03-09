using MediatR;
using ShadowNetBackend.Features.Witnesses.Common;

namespace ShadowNetBackend.Features.Witnesses.GetByIdWitness;

public record GetByIdWitnessQuery(Guid Id) : IRequest<WitnessResponse>;
