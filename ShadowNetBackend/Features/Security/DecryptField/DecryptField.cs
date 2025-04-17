namespace ShadowNetBackend.Features.Security.DecryptField;

public record DecryptFieldCommand(string Field, string? EncryptionKey, EncryptionType? EncryptionType) : ICommand<string>;

public class DecryptFieldCommandHandler : ICommandHandler<DecryptFieldCommand, string>
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

public class DecryptFieldEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/decryptfield", async (
            [FromBody] DecryptFieldCommand command,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var decryptedField = await sender.Send(command, cancellationToken);
            return TypedResults.Ok(decryptedField);
        })
        .WithTags("Security")
        .WithName("DecryptField");
    }
}