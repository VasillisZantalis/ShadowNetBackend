namespace ShadowNetBackend.Features.Security.DecryptField;

public record DecryptFieldCommand(string Field, string? EncryptionKey, EncryptionType? EncryptionType) : IRequest<string>;

public class DecryptFieldCommandHandler : IRequestHandler<DecryptFieldCommand, string>
{
    private readonly ICryptographyService _cryptographyService;
    public DecryptFieldCommandHandler(ICryptographyService cryptographyService)
    {
        _cryptographyService = cryptographyService;
    }

    public async Task<string> Handle(DecryptFieldCommand request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_cryptographyService.Decrypt(request.Field, request.EncryptionType ?? EncryptionType.None, request.EncryptionKey));
    }
}

public static class DecryptFieldEndpoints
{
    public static void MapDecryptFieldEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/decryptfield");
        group.MapPost("", DecryptField).WithName("DecryptField");
    }

    private static async Task<IResult> DecryptField(DecryptFieldCommand command, ISender sender, CancellationToken cancellationToken)
    {
        if (command is null)
            return TypedResults.BadRequest("Invalid Parameters");

        var decryptedField = await sender.Send(command, cancellationToken);

        return TypedResults.Ok(decryptedField);
    }
}
