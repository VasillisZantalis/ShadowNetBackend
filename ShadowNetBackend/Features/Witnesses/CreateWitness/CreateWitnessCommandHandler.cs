using MediatR;
using ShadowNetBackend.Helpers;
using ShadowNetBackend.Infrastructure.Data;

namespace ShadowNetBackend.Features.Witnesses.CreateWitness;

public class CreateWitnessCommandHandler : IRequestHandler<CreateWitnessCommand, Guid?>
{
    private readonly ApplicationDbContext _dbContext;

    public CreateWitnessCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid?> Handle(CreateWitnessCommand request, CancellationToken cancellationToken)
    {
        var witness = new Witness
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Alias = request.Alias ?? string.Empty,
            Image = request.Image is null
                ? null
                : FileHelper.ConvertFromBase64(request.Image),
            RiskLevel = request.RiskLevel,
            LocationHistory = request.LocationHistory,
            RelocationStatus = request.RelocationStatus
        };

        _dbContext.Witnesses.Add(witness);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return witness.Id;
    }
}
