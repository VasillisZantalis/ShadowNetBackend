
namespace ShadowNetBackend.Features.Missions;

public static class MissionEndpoints
{
    public static void MapMissionEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/missions");

        group.MapGet("", GetAllMissions).WithName("GetAllMissions");
        group.MapGet("/{id:guid}", GetMissionById).WithName("GetMissionById");
        group.MapPost("", CreateMission).WithName("CreateMission");
        group.MapPut("/{id:guid}", UpdateMission).WithName("UpdateMission");
        group.MapDelete("/{id:guid}", DeleteMission).WithName("DeleteMission");
    }

    private static async Task<IResult> GetAllMissions(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static async Task<IResult> GetMissionById(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static async Task<IResult> CreateMission(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static async Task<IResult> UpdateMission(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static async Task<IResult> DeleteMission(HttpContext context)
    {
        throw new NotImplementedException();
    }
}
