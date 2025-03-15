namespace ShadowNetBackend.Features.Auth.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, string?>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly JwtHelper _jwtHelper;

    public RefreshTokenCommandHandler(ApplicationDbContext dbContext, JwtHelper jwtHelper)
    {
        _dbContext = dbContext;
        _jwtHelper = jwtHelper;
    }

    public async Task<string?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshTokenEntity = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken, cancellationToken);

        if (refreshTokenEntity is null || refreshTokenEntity.Expires < DateTimeOffset.UtcNow)
            return null;

        var user = await _dbContext.Users.FindAsync(refreshTokenEntity.UserId);
        if (user is null)
            return null;

        return _jwtHelper.GenerateJwtToken(user);
    }
}
