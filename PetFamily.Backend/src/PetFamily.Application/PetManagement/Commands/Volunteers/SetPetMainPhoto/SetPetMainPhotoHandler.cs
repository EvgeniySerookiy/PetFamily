using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.SetPetMainPhoto;

public class SetPetMainPhotoHandler : ICommandHandler<Guid, SetPetMainPhotoCommand>
{
    private readonly IVolunteersWriteRepository _volunteersWriteRepository;
    private readonly ILogger<SetPetMainPhotoHandler> _logger;
    private readonly IValidator<SetPetMainPhotoCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public SetPetMainPhotoHandler(
        IVolunteersWriteRepository volunteersWriteRepository,
        ILogger<SetPetMainPhotoHandler> logger,
        IValidator<SetPetMainPhotoCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _volunteersWriteRepository = volunteersWriteRepository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<Guid, ErrorList>> Handle(
        SetPetMainPhotoCommand command, 
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var volunteerResult = await _volunteersWriteRepository.GetById(
            VolunteerId.Create(command.VolunteerId),
            cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var petResult = volunteerResult.Value.GetByPetId(command.PetId);
        if (petResult.IsFailure)
            return petResult.Error.ToErrorList();

        var setMainPhoto = petResult.Value.SetMainPhoto(command.PhotoPath);
        if (setMainPhoto.IsFailure)
            return setMainPhoto.Error.ToErrorList();
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation(
            "Successfully set main photo for pet with id: {PetId} to {PhotoPath} for volunteer with id: {VolunteerId}", 
            command.PetId, 
            command.PhotoPath, 
            command.VolunteerId);

        return petResult.Value.Id.Value;
    }
}