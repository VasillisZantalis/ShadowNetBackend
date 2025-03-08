using MediatR;
using Microsoft.EntityFrameworkCore;
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
        var query = _dbContext.Missions.AsQueryable();

        if (request.Parameters.Status.HasValue)
        {
            query = query.Where(w => w.Status == request.Parameters.Status.Value);
        }

        if (request.Parameters.Risk.HasValue)
        {
            query = query.Where(w => w.Risk == request.Parameters.Risk.Value);
        }

        if (request.Parameters.PageSize.HasValue && request.Parameters.PageNumber.HasValue)
        {
            int pageSize = request.Parameters.PageSize.Value;
            int pageNumber = request.Parameters.PageNumber.Value;

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
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
