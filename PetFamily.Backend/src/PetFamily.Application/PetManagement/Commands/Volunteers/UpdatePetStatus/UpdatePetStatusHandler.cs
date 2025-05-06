using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.UpdatePetStatus;

public class UpdatePetStatusHandler : ICommandHandler<Guid, UpdatePetStatusCommand>
{
    private readonly IVolunteersWriteRepository _volunteersWriteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdatePetStatusHandler> _logger;

    public UpdatePetStatusHandler(
        IVolunteersWriteRepository volunteersWriteRepository, 
        IUnitOfWork unitOfWork, 
        ILogger<UpdatePetStatusHandler> logger)
    {
        _volunteersWriteRepository = volunteersWriteRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<Result<Guid, ErrorList>> Handle(
        UpdatePetStatusCommand command, 
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersWriteRepository.GetById(command.VolunteerId);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var petResult = volunteerResult.Value.GetByPetId(command.PetId);
        if (petResult.IsFailure)
            return petResult.Error.ToErrorList();

        petResult.Value.UpdatePetStatus(command.Status);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation(
            "Updated status pet with id {PetId} from a volunteer with id {VolunteerId}", 
            command.PetId, 
            command.VolunteerId);
        
        return petResult.Value.Id.Value;
    }
}