namespace ShadowNetBackend.Features.SafeHouses.UpdateSafeHouse;

public class UpdateSafeHouseValidator : AbstractValidator<UpdateSafeHouseCommand>
{
    public UpdateSafeHouseValidator()
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
