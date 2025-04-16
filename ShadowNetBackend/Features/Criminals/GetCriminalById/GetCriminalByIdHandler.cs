using ShadowNetBackend.Features.Criminals.Common;

namespace ShadowNetBackend.Features.Criminals.GetByIdCriminal;

public record GetCriminalByIdQuery(Guid Id) : IQuery<CriminalDto>;

internal class GetCriminalByIdHandler(ApplicationDbContext dbContext) : IQueryHandler<GetCriminalByIdQuery, CriminalDto>
{
    public async Task<CriminalDto> Handle(GetCriminalByIdQuery request, CancellationToken cancellationToken)
    {
        var criminal = await dbContext.Criminals
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
       
        if (criminal is null)
            throw new CriminalNotFoundException();
        
        return criminal.ToCriminalDto();
    }
}