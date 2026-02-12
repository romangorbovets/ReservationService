using System.Reflection;
using System.Runtime.ExceptionServices;
using MediatR;
using Microsoft.Extensions.Logging;
using ReservationService.Application.Common.Interfaces;
using ReservationService.Application.Common.MediatR;
using ReservationService.Domain.Common.Exceptions;

namespace ReservationService.Application.Common.Services;

public class CommandSender : ICommandSender
{
    private readonly IMediator _mediator;
    private readonly ILogger<CommandSender> _logger;

    public CommandSender(IMediator mediator, ILogger<CommandSender> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<TResponse> Send<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand<TResponse>
    {
        try
        {
            var adapter = new CommandMediatRAdapter<TCommand, TResponse>(command);
            return await _mediator.Send(adapter, cancellationToken);
        }
        catch (ValidationException)
        {
            
            throw;
        }
        catch (KeyNotFoundException)
        {
           
            throw;
        }
        catch (TargetInvocationException ex) when (ex.InnerException is not null)
        {
            _logger.LogDebug("Перехвачено TargetInvocationException в Send<TCommand, TResponse>, пробрасываю InnerException: {Type}", ex.InnerException.GetType().Name);
            
            
            if (ex.InnerException is ValidationException validationEx)
            {
                throw validationEx;
            }
            if (ex.InnerException is KeyNotFoundException keyNotFoundEx)
            {
                throw keyNotFoundEx;
            }
            
            ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            throw;
        }
        catch (AggregateException ex)
        {
            _logger.LogDebug("Перехвачено AggregateException в Send<TCommand, TResponse> с {Count} внутренними исключениями", ex.InnerExceptions.Count);
            if (ex.InnerExceptions.Count == 1)
            {
                _logger.LogDebug("Пробрасываю единственное внутреннее исключение: {Type}", ex.InnerException!.GetType().Name);
                
                
                if (ex.InnerException is ValidationException validationEx)
                {
                    throw validationEx;
                }
                if (ex.InnerException is KeyNotFoundException keyNotFoundEx)
                {
                    throw keyNotFoundEx;
                }
                
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogDebug("Исключение в Send<TCommand, TResponse>: {Type}, {Message}", ex.GetType().Name, ex.Message);
            throw;
        }
    }

    public async Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        try
        {
            var commandType = command.GetType();
            var genericMethod = typeof(CommandSender)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(m => m.Name == nameof(Send) && 
                                     m.IsGenericMethod && 
                                     m.GetGenericArguments().Length == 2 &&
                                     m.GetParameters().Length == 2 &&
                                     m.GetParameters()[0].ParameterType.IsGenericParameter);

            if (genericMethod is null)
            {
                throw new InvalidOperationException($"Unable to find Send method for command type {commandType.Name}.");
            }

            var constructedMethod = genericMethod.MakeGenericMethod(commandType, typeof(TResponse));
            var invokeResult = constructedMethod.Invoke(this, new object[] { command, cancellationToken });
            
            if (invokeResult is null)
            {
                throw new InvalidOperationException($"Send method returned null for command type {commandType.Name}.");
            }

            var task = (Task<TResponse>)invokeResult;
            return await task;
        }
        catch (ValidationException)
        {
            
            throw;
        }
        catch (KeyNotFoundException)
        {
            
            throw;
        }
        catch (TargetInvocationException ex) when (ex.InnerException is not null)
        {
            _logger.LogDebug("Перехвачено TargetInvocationException, пробрасываю InnerException: {Type}", ex.InnerException.GetType().Name);
            
            
            if (ex.InnerException is ValidationException validationEx)
            {
                throw validationEx;
            }
            if (ex.InnerException is KeyNotFoundException keyNotFoundEx)
            {
                throw keyNotFoundEx;
            }
            
            ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            throw;
        }
        catch (AggregateException ex)
        {
            _logger.LogDebug("Перехвачено AggregateException с {Count} внутренними исключениями", ex.InnerExceptions.Count);
            if (ex.InnerExceptions.Count == 1)
            {
                _logger.LogDebug("Пробрасываю единственное внутреннее исключение: {Type}", ex.InnerException!.GetType().Name);
                
                
                if (ex.InnerException is ValidationException validationEx)
                {
                    throw validationEx;
                }
                if (ex.InnerException is KeyNotFoundException keyNotFoundEx)
                {
                    throw keyNotFoundEx;
                }
                
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
            throw;
        }
        catch (InvalidCastException ex)
        {
            throw new InvalidOperationException($"Failed to cast result to Task<TResponse> for command type {command.GetType().Name}.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Неожиданное исключение в CommandSender.Send: {Type}, {Message}", ex.GetType().Name, ex.Message);
            throw;
        }
    }

    public async Task Send<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand
    {
        try
        {
            var adapter = new CommandMediatRAdapter<TCommand>(command);
            await _mediator.Send(adapter, cancellationToken);
        }
        catch (ValidationException)
        {
            
            throw;
        }
        catch (TargetInvocationException ex) when (ex.InnerException is not null)
        {
            _logger.LogDebug("Перехвачено TargetInvocationException в Send<TCommand>, пробрасываю InnerException: {Type}", ex.InnerException.GetType().Name);
            
            
            if (ex.InnerException is ValidationException validationEx)
            {
                throw validationEx;
            }
            
            ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            throw;
        }
        catch (AggregateException ex)
        {
            _logger.LogDebug("Перехвачено AggregateException в Send<TCommand> с {Count} внутренними исключениями", ex.InnerExceptions.Count);
            if (ex.InnerExceptions.Count == 1)
            {
                _logger.LogDebug("Пробрасываю единственное внутреннее исключение: {Type}", ex.InnerException!.GetType().Name);
                
                
                if (ex.InnerException is ValidationException validationEx)
                {
                    throw validationEx;
                }
                
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogDebug("Исключение в Send<TCommand>: {Type}, {Message}", ex.GetType().Name, ex.Message);
            throw;
        }
    }

    public async Task Send(ICommand command, CancellationToken cancellationToken = default)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        try
        {
            var commandType = command.GetType();
            var genericMethod = typeof(CommandSender)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(m => m.Name == nameof(Send) && 
                                     m.IsGenericMethod && 
                                     m.GetGenericArguments().Length == 1 &&
                                     m.GetParameters().Length == 2 &&
                                     m.GetParameters()[0].ParameterType.IsGenericParameter);

            if (genericMethod is null)
            {
                throw new InvalidOperationException($"Unable to find Send method for command type {commandType.Name}.");
            }

            var constructedMethod = genericMethod.MakeGenericMethod(commandType);
            var invokeResult = constructedMethod.Invoke(this, new object[] { command, cancellationToken });
            
            if (invokeResult is null)
            {
                throw new InvalidOperationException($"Send method returned null for command type {commandType.Name}.");
            }

            var task = (Task)invokeResult;
            await task;
        }
        catch (ValidationException)
        {
            
            throw;
        }
        catch (TargetInvocationException ex) when (ex.InnerException is not null)
        {
            _logger.LogDebug("Перехвачено TargetInvocationException, пробрасываю InnerException: {Type}", ex.InnerException.GetType().Name);
            
            
            if (ex.InnerException is ValidationException validationEx)
            {
                throw validationEx;
            }
            
            ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            throw;
        }
        catch (AggregateException ex)
        {
            _logger.LogDebug("Перехвачено AggregateException с {Count} внутренними исключениями", ex.InnerExceptions.Count);
            if (ex.InnerExceptions.Count == 1)
            {
                _logger.LogDebug("Пробрасываю единственное внутреннее исключение: {Type}", ex.InnerException!.GetType().Name);
                
                
                if (ex.InnerException is ValidationException validationEx)
                {
                    throw validationEx;
                }
                
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
            throw;
        }
        catch (InvalidCastException ex)
        {
            throw new InvalidOperationException($"Failed to cast result to Task for command type {command.GetType().Name}.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Неожиданное исключение в CommandSender.Send: {Type}, {Message}", ex.GetType().Name, ex.Message);
            throw;
        }
    }
}