using Microsoft.AspNetCore.Identity;
using ShadowNetBackend.Common.Helpers;

namespace ShadowNetBackend.Features.Auth.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, object?>
{
    private readonly UserManager<Agent> _userManager;
    private readonly ApplicationDbContext _dbContext;
    private readonly JwtHelper _jwtHelper;

    public LoginCommandHandler(UserManager<Agent> userManager, ApplicationDbContext dbContext, JwtHelper jwtHelper)
    {
        _userManager = userManager;
        _dbContext = dbContext;
        _jwtHelper = jwtHelper;
    }

    public async Task<object?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
        {
            var accessToken = _jwtHelper.GenerateJwtToken(user);
            var refreshToken = _jwtHelper.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken.RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                Expires = DateTimeOffset.UtcNow.AddDays(7),
            };

            _dbContext.RefreshTokens.Add(refreshTokenEntity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new { AccessToken = accessToken, RefreshToken = refreshToken };
        }

        return null;
    }
}