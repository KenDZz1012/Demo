using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorize.Application.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
                if (failures.Count != 0)
                {
                    var errorMessages = failures.Select(f => $"{f.PropertyName}: {f.ErrorMessage}").ToList();
                    var errorMessage = "Validation failed";
                    var responseType = typeof(TResponse);
                    var failureMethod = responseType.GetMethod("Failure", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    var response = failureMethod.Invoke(null, new object[] { "400", errorMessage, errorMessages });

                    return (TResponse)response;
                }
            }
            return await next();
        }
    }
}
