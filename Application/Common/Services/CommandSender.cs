using MediatR;
using ReservationService.Application.Common.Interfaces;
using ReservationService.Application.Common.MediatR;

namespace ReservationService.Application.Common.Services;

public class CommandSender : ICommandSender
{
    private readonly IMediator _mediator;

    public CommandSender(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<TResponse> Send<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand<TResponse>
    {
        var adapter = new CommandMediatRAdapter<TCommand, TResponse>(command);
        return await _mediator.Send(adapter, cancellationToken);
    }

    public async Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
    {
        return await Send((dynamic)command, cancellationToken);
    }

    public async Task Send<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand
    {
        var adapter = new CommandMediatRAdapter<TCommand>(command);
        await _mediator.Send(adapter, cancellationToken);
    }

    public async Task Send(ICommand command, CancellationToken cancellationToken = default)
    {
        await Send((dynamic)command, cancellationToken);
    }
}