using Ardalis.Result;
using FluentValidation;
using MediatR;
using Ardalis.Result.FluentValidation;

namespace Zen.Application.MediatR.Behaviors;

public class ZenValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken))
        );

        var errors = validationResults
            .Where(r => r.Errors.Count > 0)
            .SelectMany(r => r.AsErrors())
            .ToList();

        if (errors.Count != 0)
        {
            // Support Result (non-generic)
            if (typeof(TResponse) == typeof(Result))
                return (TResponse)(object)Result.Invalid(errors);

            // Support Result<T>
            else if (typeof(TResponse).IsGenericType &&
                typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var genericType = typeof(TResponse).GenericTypeArguments[0];
                var invalidMethod = typeof(Result<>)
                    .MakeGenericType(genericType)
                    .GetMethod(nameof(Result<object>.Invalid), [typeof(IEnumerable<ValidationError>)]);

                var invalidResult = invalidMethod!.Invoke(null, [errors]);
                return (TResponse)invalidResult!;
            }

            // Not supported
            throw new InvalidOperationException("TResponse must be Result or Result<T>.");
        }

        return await next();
    }
}