using MediatR;
using ReservationService.Application.Common.Interfaces;

namespace ReservationService.Application.Common.MediatR;

public class CommandMediatRAdapter<TCommand, TResponse> : IRequest<TResponse>
    where TCommand : ICommand<TResponse>
{
    public TCommand Command { get; }

    public CommandMediatRAdapter(TCommand command)
    {
        Command = command;
    }
}

public class CommandMediatRAdapter<TCommand> : IRequest
    where TCommand : ICommand
{
    public TCommand Command { get; }

    public CommandMediatRAdapter(TCommand command)
    {
        Command = command;
    }
}

public class CommandHandlerMediatRAdapter<TCommand, TResponse> : IRequestHandler<CommandMediatRAdapter<TCommand, TResponse>, TResponse>
    where TCommand : ICommand<TResponse>
{
    private readonly ICommandHandler<TCommand, TResponse> _handler;

    public CommandHandlerMediatRAdapter(ICommandHandler<TCommand, TResponse> handler)
    {
        _handler = handler;
    }

    public async Task<TResponse> Handle(CommandMediatRAdapter<TCommand, TResponse> request, CancellationToken cancellationToken)
    {
        return await _handler.Handle(request.Command, cancellationToken);
    }
}

public class CommandHandlerMediatRAdapter<TCommand> : IRequestHandler<CommandMediatRAdapter<TCommand>>
    where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> _handler;

    public CommandHandlerMediatRAdapter(ICommandHandler<TCommand> handler)
    {
        _handler = handler;
    }

    public async Task Handle(CommandMediatRAdapter<TCommand> request, CancellationToken cancellationToken)
    {
        await _handler.Handle(request.Command, cancellationToken);
    }
}