using ShadowNetBackend.Features.Witnesses.Common;

namespace ShadowNetBackend.Features.Witnesses.GetByIdWitness;

public class GetByIdWitnessQueryHandler : IRequestHandler<GetByIdWitnessQuery, WitnessResponse>
{
    private readonly ApplicationDbContext _dbContext;

    public GetByIdWitnessQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<WitnessResponse> Handle(GetByIdWitnessQuery request, CancellationToken cancellationToken)
    {
        var witness = await _dbContext.Witnesses
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (witness is null)
            throw new WitnessNotFoundException();

        var witnessResponse = witness.ToWitnessResponse();

        return witnessResponse;
    }
}
