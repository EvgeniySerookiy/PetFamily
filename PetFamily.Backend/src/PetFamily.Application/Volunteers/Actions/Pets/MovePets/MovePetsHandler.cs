using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Volunteers.Actions.Pets.AddPet;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Volunteers.Actions.Pets.MovePets;

public class MovePetsHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<AddPetHandler> _logger;

    public MovePetsHandler(
        IUnitOfWork unitOfWork,
        IVolunteersRepository volunteersRepository,
        ILogger<AddPetHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        MovePetsCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersRepository.GetById(
            VolunteerId.Create(command.id),
            cancellationToken);

        var petMoveResult = volunteerResult.Value.Pets
            .FirstOrDefault(p => p.SerialNumber.Value == command.CurrentPosition);
        if (petMoveResult == null)
        {
            _logger.LogWarning("Pet with serial number {SerialNumber} not found", 
                command.CurrentPosition);
            
            return Errors.General.InvalidRequest(command.CurrentPosition);
        }
        
        volunteerResult.Value.MovePet(petMoveResult, command.ToPosition);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Moving pet with ID {PetId} for volunteer with ID {VolunteerId}", 
            petMoveResult.Id, volunteerResult.Value.Id);
        
        return petMoveResult.Id.Value;
    }
}