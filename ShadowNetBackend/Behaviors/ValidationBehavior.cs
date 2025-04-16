using ShadowNetBackend.Exceptions;

namespace ShadowNetBackend.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (validator != null)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
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
