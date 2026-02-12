using System.Reflection;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using ReservationService.Application.Common.MediatR;
using DomainValidationException = ReservationService.Domain.Common.Exceptions.ValidationException;

namespace ReservationService.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

    public ValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators,
        IServiceProvider serviceProvider,
        ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    {
        _validators = validators;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        object? commandToValidate = request;

        
        var requestType = request.GetType();
        if (requestType.IsGenericType)
        {
            var genericTypeDefinition = requestType.GetGenericTypeDefinition();
            if (genericTypeDefinition == typeof(CommandMediatRAdapter<,>) || 
                genericTypeDefinition == typeof(CommandMediatRAdapter<>))
            {
                var commandProperty = requestType.GetProperty("Command");
                commandToValidate = commandProperty?.GetValue(request);
            }
        }

        if (commandToValidate is null)
        {
            return await next();
        }

        
        var commandType = commandToValidate.GetType();
        var validatorType = typeof(IValidator<>).MakeGenericType(commandType);
        var validators = _serviceProvider.GetServices(validatorType).Cast<IValidator>();

        if (!validators.Any())
        {
            return await next();
        }

       
        var validationContextType = typeof(ValidationContext<>).MakeGenericType(commandType);
        var validationContext = Activator.CreateInstance(validationContextType, commandToValidate);

        var validationTasks = validators.Select(v =>
        {
            var validateMethod = v.GetType().GetMethod("ValidateAsync", new[] { validationContextType, typeof(CancellationToken) });
            return (Task<ValidationResult>)validateMethod!.Invoke(v, new[] { validationContext, cancellationToken })!;
        });

        var validationResults = await Task.WhenAll(validationTasks);

        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Any())
        {
            var errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(g => g.Key, g => g.ToArray());

            _logger.LogWarning("Валидация не пройдена. Ошибки: {Errors}", string.Join(", ", errors.SelectMany(e => e.Value)));
            
            var exception = new DomainValidationException(errors);
            throw exception;
        }

        return await next();
    }
}