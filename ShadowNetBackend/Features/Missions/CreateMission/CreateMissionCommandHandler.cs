using MediatR;
using ShadowNetBackend.Common;
using ShadowNetBackend.Helpers;
using ShadowNetBackend.Infrastructure.Data;
using ShadowNetBackend.Infrastructure.Security;

namespace ShadowNetBackend.Features.Missions.CreateMission;

public class CreateMissionCommandHandler : IRequestHandler<CreateMissionCommand, Guid>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICryptographyService _cryptographyService;

    public CreateMissionCommandHandler(ApplicationDbContext dbContext, ICryptographyService cryptographyService)
    {
        _dbContext = dbContext;
        _cryptographyService = cryptographyService;
    }

    public async Task<Guid> Handle(CreateMissionCommand request, CancellationToken cancellationToken)
    {
        var mission = new Mission
        {
            Title = request.Title,
            Image = request.Image != null 
                ? FileHelper.ConvertFromBase64(request.Image) 
                : null,
            Objective = request.EncryptionType != EncryptionType.None
                ? _cryptographyService.Encrypt(request.Objective, request.EncryptionType)
                : request.Objective,
            Location = request.EncryptionType != EncryptionType.None
                ? _cryptographyService.Encrypt(request.Location, request.EncryptionType)
                : request.Location,
            Status = request.Status,
            Risk = request.Risk,
            Date = request.Date
        };

        _dbContext.Missions.Add(mission);
        await _dbContext.SaveChangesAsync();

        return mission.Id;
    }
}
