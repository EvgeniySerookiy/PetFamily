using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.MovePets;

public class MovePetsHandler : ICommandHandler<Guid, MovePetsCommand>
{
    private readonly IVolunteersWriteRepository _volunteersWriteRepository;
    private readonly ILogger<AddPet.AddPet> _logger;
    private readonly IValidator<MovePetsCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public MovePetsHandler(
        IVolunteersWriteRepository volunteersWriteRepository,
        ILogger<AddPet.AddPet> logger,
        IValidator<MovePetsCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _volunteersWriteRepository = volunteersWriteRepository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        MovePetsCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var volunteerResult = await _volunteersWriteRepository.GetById(
            VolunteerId.Create(command.Id),
            cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var petMoveResult = volunteerResult.Value.Pets
            .FirstOrDefault(p => p.Position.Value == command.CurrentPosition);
        if (petMoveResult == null)
        {
            _logger.LogWarning("Pet with serial number {SerialNumber} not found", 
                command.CurrentPosition);
            
            return Errors.General.InvalidRequest(command.CurrentPosition).ToErrorList();
        }
        
        volunteerResult.Value.MovePet(petMoveResult, command.ToPosition);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Moving pet with ID {PetId} for volunteer with ID {VolunteerId}", 
            petMoveResult.Id, volunteerResult.Value.Id);
        
        return petMoveResult.Id.Value;
    }
}