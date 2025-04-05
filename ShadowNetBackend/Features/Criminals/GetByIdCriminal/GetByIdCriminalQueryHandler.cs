using ShadowNetBackend.Features.Criminals.Common;

namespace ShadowNetBackend.Features.Criminals.GetByIdCriminal;

public class GetByIdCriminalQueryHandler : IRequestHandler<GetByIdCriminalQuery, CriminalResponse>
{
    private readonly ApplicationDbContext _dbContext;
    public GetByIdCriminalQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CriminalResponse> Handle(GetByIdCriminalQuery request, CancellationToken cancellationToken)
    {
        var criminal = await _dbContext.Criminals
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
       
        if (criminal is null)
            throw new CriminalNotFoundException();
        
        return criminal.ToCriminalResponse();
    }
}