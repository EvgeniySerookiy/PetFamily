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
            VolunteerId.Create(command.VolunteerId),
            cancellationToken);

        var petMoveResult = volunteerResult.Value.GetPetById(command.PetIdMove);
        var petTargetResult = volunteerResult.Value.GetPetById(command.PetIdTarget);
        
        volunteerResult.Value.MovePet(petMoveResult.Value, petTargetResult.Value.SerialNumber.Value);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Moving pet with ID {PetId} for volunteer with ID {VolunteerId}", 
            petMoveResult.Value.Id, volunteerResult.Value.Id);
        
        return petMoveResult.Value.Id.Value;
    }
}