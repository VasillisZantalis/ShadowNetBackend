using Newtonsoft.Json.Linq;
using ShadowNetBackend.Features.Auth.Login;
using ShadowNetBackend.Features.Auth.RefreshToken;

namespace ShadowNetBackend.Features.Auth;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Auth");

        group.MapPost("/login", Login).WithName("login");
        group.MapPost("/refresh-token", RefreshToken).WithName("refresh-token");
    }

    private static async Task<IResult> Login(LoginCommand command, ISender sender, CancellationToken cancellationToken)
    {
        var token = await sender.Send(command, cancellationToken);

        return token is null
            ? TypedResults.BadRequest("Failed attempt to login")
            : TypedResults.Ok(token);
    }

    private static async Task<IResult> RefreshToken(RefreshTokenCommand command, ISender sender, CancellationToken cancellationToken)
    {
        var token = await sender.Send(command, cancellationToken);
        return token is null
            ? TypedResults.BadRequest("Invalid refresh token")
            : TypedResults.Ok(new { AccessToken = token });
    }
}
