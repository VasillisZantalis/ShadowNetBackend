using ShadowNetBackend.Exceptions;

namespace ShadowNetBackend.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>
{
    private readonly IValidator<TRequest>? _validator;

    public ValidationBehavior(IValidator<TRequest>? validator = null)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validator != null)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                   .GroupBy(e => e.PropertyName)
                   .ToDictionary(
                       g => g.Key,
                       g => g.Select(e => e.ErrorMessage).ToArray()
                   );

                throw new ValidationFailedException(errors);
            }
        }

        return await next();
    }
}
