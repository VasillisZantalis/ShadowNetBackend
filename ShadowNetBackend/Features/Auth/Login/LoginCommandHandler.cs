using Microsoft.AspNetCore.Identity;

namespace ShadowNetBackend.Features.Auth.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, string?>
{
    private readonly UserManager<Agent> _userManager;
    private readonly JwtHelper _jwtHelper;

    public LoginCommandHandler(UserManager<Agent> userManager, JwtHelper jwtHelper)
    {
        _userManager = userManager;
        _jwtHelper = jwtHelper;
    }

    public async Task<string?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
        {
            var token = _jwtHelper.GenerateJwtToken(user);
            return token;
        }

        return null;
    }
}