using ShadowNetBackend.Features.Auth.Login;

namespace ShadowNetBackend.Features.Auth;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapPost("/login", Login).WithName("login");
    }

    private static async Task<IResult> Login(LoginCommand command, ISender sender, CancellationToken cancellationToken)
    {
        var token = await sender.Send(command, cancellationToken);

        return token is null
            ? TypedResults.BadRequest("Failed attempt to login")
            : TypedResults.Ok(new { Token = token });
    }
}
