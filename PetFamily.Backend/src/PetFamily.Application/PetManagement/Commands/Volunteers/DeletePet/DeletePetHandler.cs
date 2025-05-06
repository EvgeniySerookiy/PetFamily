using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.DeletePet;

public class DeletePetHandler : ICommandHandler<Guid, DeletePetCommand>
{
    private readonly IVolunteersWriteRepository _volunteersWriteRepository;
    private readonly ILogger<DeletePetCommand> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePetHandler(
        IVolunteersWriteRepository volunteersWriteRepository,
        ILogger<DeletePetCommand> logger,
        IUnitOfWork unitOfWork)
    {
        _volunteersWriteRepository = volunteersWriteRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        DeletePetCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersWriteRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var petResult = volunteerResult.Value.GetByPetId(command.PetId);
        if (petResult.IsFailure)
            return petResult.Error.ToErrorList();

        petResult.Value.Delete();
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation(
            "Successfully deleted pet with id: {PetId} for volunteer with id: {VolunteerId}",
            command.PetId, command.VolunteerId);

        return command.PetId;
    }
}