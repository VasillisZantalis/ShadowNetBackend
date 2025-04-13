namespace ShadowNetBackend.Features.SafeHouses.CreateSafeHouse;

public class CreateSafeHouseValidator : AbstractValidator<CreateSafeHouseCommand>
{
    public CreateSafeHouseValidator()
    {
        RuleFor(x => x.Location)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Capacity)
            .NotEmpty();

        RuleFor(x => x.IsActive)
            .NotNull();
    }
}
