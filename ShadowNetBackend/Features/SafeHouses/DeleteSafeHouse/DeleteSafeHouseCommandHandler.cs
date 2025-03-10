using MediatR;
using ShadowNetBackend.Features.SafeHouses.GetByIdSafeHouse;
using ShadowNetBackend.Infrastructure.Data;

namespace ShadowNetBackend.Features.SafeHouses.DeleteSafeHouse;

public class DeleteSafeHouseCommandHandler : IRequestHandler<DeleteSafeHouseCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISender _sender;

    public DeleteSafeHouseCommandHandler(ApplicationDbContext dbContext, ISender sender)
    {
        _dbContext = dbContext;
        _sender = sender;
    }

    public async Task<bool> Handle(DeleteSafeHouseCommand request, CancellationToken cancellationToken)
    {
        await _sender.Send(new GetByIdSafeHouseQuery(request.Id), cancellationToken);

        var SafeHouse = await _dbContext.SafeHouses.FindAsync(request.Id);

        _dbContext.SafeHouses.Remove(SafeHouse!);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
