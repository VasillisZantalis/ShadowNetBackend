using MediatR;
using Microsoft.EntityFrameworkCore;
using ShadowNetBackend.Features.Witnesses.Common;
using ShadowNetBackend.Infrastructure.Data;
using ShadowNetBackend.Mappings;

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

        return witness is null
            ? throw new WitnessNotFoundException()
            : witness.ToWitnessResponse();
    }
}
