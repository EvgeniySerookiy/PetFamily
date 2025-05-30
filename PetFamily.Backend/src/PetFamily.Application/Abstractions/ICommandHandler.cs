using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Abstractions;

public interface ICommandHandler<TResponse, in TCommand> where TCommand : ICommand
{
    Task<Result<TResponse, ErrorList>> Handle(
        TCommand command,
        CancellationToken cancellationToken = default);
}

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task<Result<UnitResult<ErrorList>>> Handle(
        TCommand command,
        CancellationToken cancellationToken = default);
}