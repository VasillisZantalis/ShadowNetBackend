using MediatR;
using Microsoft.EntityFrameworkCore;
using ShadowNetBackend.Features.Witnesses.GetByIdWitness;
using ShadowNetBackend.Infrastructure.Data;

namespace ShadowNetBackend.Features.Witnesses.DeleteWitness;

public class DeleteWitnessCommandHandler : IRequestHandler<DeleteWitnessCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISender _sender;

    public DeleteWitnessCommandHandler(ApplicationDbContext dbContext, ISender sender)
    {
        _dbContext = dbContext;
        _sender = sender;
    }

    public async Task<bool> Handle(DeleteWitnessCommand request, CancellationToken cancellationToken)
    {
        await _sender.Send(new GetByIdWitnessQuery(request.Id), cancellationToken);

        var witness = await _dbContext.Witnesses.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        _dbContext.Witnesses.Remove(witness!);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
