using MediatR;
using Microsoft.EntityFrameworkCore;
using ShadowNetBackend.Features.Agents.Common;
using ShadowNetBackend.Features.Witnesses.Common;
using ShadowNetBackend.Infrastructure.Data;
using ShadowNetBackend.Mappings;

namespace ShadowNetBackend.Features.Witnesses.GetAllWitnesses;

public class GetWitnessesQueryHandler : IRequestHandler<GetWitnessesQuery, IEnumerable<WitnessResponse>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetWitnessesQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<WitnessResponse>> Handle(GetWitnessesQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Witnesses.AsQueryable();

        if (!string.IsNullOrEmpty(request.Parameters.Name))
        {
            query = query.Where(w => request.Parameters.Name.Contains(w.FirstName) || request.Parameters.Name.Contains(w.LastName));
        }

        if (!string.IsNullOrEmpty(request.Parameters.Alias))
        {
            query = query.Where(w => w.Alias != null 
                && (request.Parameters.Alias.Contains(w.Alias) 
                    || request.Parameters.Alias.Contains(w.Alias)));
        }

        if (request.Parameters.PageSize.HasValue && request.Parameters.PageNumber.HasValue)
        {
            int pageSize = request.Parameters.PageSize.Value;
            int pageNumber = request.Parameters.PageNumber.Value;

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        var witnesses = await query.ToListAsync(cancellationToken);

        return witnesses.Select(s => s.ToWitnessResponse()).ToList();
    }
}
