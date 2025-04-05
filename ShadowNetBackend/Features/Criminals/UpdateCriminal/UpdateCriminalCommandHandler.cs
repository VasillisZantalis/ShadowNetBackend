using ShadowNetBackend.Features.Criminals.Common;

namespace ShadowNetBackend.Features.Criminals.UpdateCriminal;

public class UpdateCriminalCommandHandler : IRequestHandler<UpdateCriminalCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICacheService _cache;

    public UpdateCriminalCommandHandler(ApplicationDbContext dbContext, ICacheService cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<bool> Handle(UpdateCriminalCommand request, CancellationToken cancellationToken)
    {

        var criminal = await _dbContext.Criminals.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (criminal is null)
            throw new CriminalNotFoundException();

        criminal.FirstName = request.FirstName;
        criminal.LastName = request.LastName;
        criminal.Alias = request.Alias;
        criminal.DateOfBirth = request.DateOfBirth;
        criminal.Nationality = request.Nationality;
        criminal.KnownAffiliations = request.KnownAffiliations;
        criminal.ThreatLevel = request.ThreatLevel;
        criminal.IsArmedAndDangerous = request.IsArmedAndDangerous;
        criminal.LastKnownLocation = request.LastKnownLocation;
        criminal.LastSpottedDate = request.LastSpottedDate;
        criminal.UnderSurveillance = request.UnderSurveillance;
        criminal.SurveillanceNotes = request.SurveillanceNotes;
        criminal.Image = request.Image != null ? FileHelper.ConvertFromBase64(request.Image) : null;

        await _dbContext.SaveChangesAsync(cancellationToken);
        await _cache.RemoveAsync(nameof(CacheKeys.Criminals));

        return true;
    }
}
