using MediatR;
using Microsoft.EntityFrameworkCore;
using ShadowNetBackend.Features.SafeHouses.GetByIdSafeHouse;
using ShadowNetBackend.Infrastructure.Data;

namespace ShadowNetBackend.Features.SafeHouses.UpdateSafeHouse;

public class UpdateSafeHouseCommandHandler : IRequestHandler<UpdateSafeHouseCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISender _sender;

    public UpdateSafeHouseCommandHandler(ApplicationDbContext dbContext, ISender sender)
    {
        _dbContext = dbContext;
        _sender = sender;
    }

    public async Task<bool> Handle(UpdateSafeHouseCommand request, CancellationToken cancellationToken)
    {
        await _sender.Send(new GetByIdSafeHouseQuery(request.Id), cancellationToken);

        var safeHouse = await _dbContext.SafeHouses.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        safeHouse!.Location = request.Location;
        safeHouse.Capacity = request.Capacity;
        safeHouse.IsActive = request.IsActive;

        _dbContext.SafeHouses.Update(safeHouse);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
