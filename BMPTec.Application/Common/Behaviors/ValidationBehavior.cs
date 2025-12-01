using FluentValidation;
using MediatR;

namespace BMPTec.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
      where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                var erros = failures.Select(f => f.ErrorMessage).ToList();

                // Se TResponse é Result ou Result<T>, retorna um resultado com falha
                if (typeof(TResponse).IsGenericType &&
                    typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
                {
                    var resultType = typeof(TResponse);
                    var failureMethod = resultType.GetMethod("Failure", new[] { typeof(List<string>) });

                    if (failureMethod != null)
                    {
                        var result = failureMethod.Invoke(null, new object[] { erros });
                        return (TResponse)result!;
                    }
                }

                throw new ValidationException(failures);
            }

            return await next();
        }
    }
}
