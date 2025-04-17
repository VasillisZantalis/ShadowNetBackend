using ShadowNetBackend.Features.Witnesses.Common;

namespace ShadowNetBackend.Features.Witnesses.GetByIdWitness;

public record GetWitnessByIdQuery(Guid Id) : IQuery<WitnessDto>;

internal class GetWitnessByIdHandler(
    ApplicationDbContext dbContext) : IQueryHandler<GetWitnessByIdQuery, WitnessDto>
{
    public async Task<WitnessDto> Handle(GetWitnessByIdQuery request, CancellationToken cancellationToken)
    {
        var witness = await dbContext.Witnesses
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (witness is null)
            throw new WitnessNotFoundException();

        return witness.ToWitnessDto();
    }
}