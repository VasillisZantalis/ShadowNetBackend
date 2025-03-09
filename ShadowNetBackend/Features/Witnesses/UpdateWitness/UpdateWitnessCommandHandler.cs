using MediatR;
using Microsoft.EntityFrameworkCore;
using ShadowNetBackend.Features.Missions.GetByIdMission;
using ShadowNetBackend.Features.Witnesses.GetByIdWitness;
using ShadowNetBackend.Helpers;
using ShadowNetBackend.Infrastructure.Data;

namespace ShadowNetBackend.Features.Witnesses.UpdateWitness;

public class UpdateWitnessCommandHandler : IRequestHandler<UpdateWitnessCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISender _sender;

    public UpdateWitnessCommandHandler(ApplicationDbContext dbContext, ISender sender)
    {
        _dbContext = dbContext;
        _sender = sender;
    }

    public async Task<bool> Handle(UpdateWitnessCommand request, CancellationToken cancellationToken)
    {
        await _sender.Send(new GetByIdWitnessQuery(request.Id), cancellationToken);

        var witness = await _dbContext.Witnesses.FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

        witness!.Alias = request.Alias ?? string.Empty;
        witness.Image = request.Image is null
                ? null
                : FileHelper.ConvertFromBase64(request.Image);
        witness.RiskLevel = request.RiskLevel;
        witness.LocationHistory = request.LocationHistory;
        witness.RelocationStatus = request.RelocationStatus;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
