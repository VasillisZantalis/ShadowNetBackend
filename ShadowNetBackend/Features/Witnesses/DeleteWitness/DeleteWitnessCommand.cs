namespace ShadowNetBackend.Features.Witnesses.DeleteWitness;

public record DeleteWitnessCommand(Guid Id) : IRequest<bool>;
