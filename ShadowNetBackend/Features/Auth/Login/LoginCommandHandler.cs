using Microsoft.AspNetCore.Identity;

namespace ShadowNetBackend.Features.Auth.Login;

public class LoginCommandHandler(UserManager<Agent> userManager, ApplicationDbContext dbContext, JwtHelper jwtHelper) : IRequestHandler<LoginCommand, object?>
{
    public async Task<object?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user != null && await userManager.CheckPasswordAsync(user, request.Password))
        {
            var accessToken = jwtHelper.GenerateJwtToken(user);
            var refreshToken = jwtHelper.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken.RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                Expires = DateTimeOffset.UtcNow.AddDays(7),
            };

            dbContext.RefreshTokens.Add(refreshTokenEntity);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new { AccessToken = accessToken, RefreshToken = refreshToken };
        }

        return null;
    }
}