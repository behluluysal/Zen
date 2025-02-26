using Zen.Application.Utilities.Common;

namespace Zen.Application.MediatR.Command;

/// <summary>
/// Marker interface for commands.
/// </summary>
public interface ICommand { }

/// <summary>
/// Handles commands of type TCommand.
/// </summary>
public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    Task<OperationResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}