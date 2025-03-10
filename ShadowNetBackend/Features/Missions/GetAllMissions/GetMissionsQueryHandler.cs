using MediatR;
using Microsoft.EntityFrameworkCore;
using ShadowNetBackend.Extensions;
using ShadowNetBackend.Features.Missions.Common;
using ShadowNetBackend.Helpers;
using ShadowNetBackend.Infrastructure.Data;

namespace ShadowNetBackend.Features.Missions.GetAllMissions;

public class GetMissionsQueryHandler : IRequestHandler<GetMissionsQuery, IEnumerable<MissionResponse>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetMissionsQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<MissionResponse>> Handle(GetMissionsQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Missions.AsQueryable()
            .ApplySorting(request.Parameters.OrderBy)
            .ApplyPagination(request.Parameters.PageSize, request.Parameters.PageNumber);

        if (request.Parameters.Status.HasValue)
        {
            query = query.Where(w => w.Status == request.Parameters.Status.Value);
        }

        if (request.Parameters.Risk.HasValue)
        {
            query = query.Where(w => w.Risk == request.Parameters.Risk.Value);
        }

        var missions = await query.ToListAsync(cancellationToken);

        return missions.Select(mission => new MissionResponse
        {
            Id = mission.Id,
            Title = mission.Title,
            Image = mission.Image != null
                ? FileHelper.ConvertToBase64(mission.Image)
                : null,
            Objective = mission.Objective,
            Location = mission.Location,
            Status = mission.Status,
            Risk = mission.Risk,
            Date = mission.Date
        }).ToList();
    }
}
