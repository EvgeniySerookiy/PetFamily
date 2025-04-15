using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.PetVO;
using PetFamily.Domain.PetManagement.SharedVO;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SpeciesManagement.SpeciesVO;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.AddPet;

public class AddPetHandler : ICommandHandler<Guid, MainPetInfoCommand>
{
    private readonly IReadDbContext _readDbContext;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<AddPetHandler> _logger;
    private readonly IValidator<MainPetInfoCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public AddPetHandler(
        IReadDbContext readDbContext,
        IVolunteersRepository volunteersRepository,
        ILogger<AddPetHandler> logger,
        IValidator<MainPetInfoCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _readDbContext = readDbContext;
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        MainPetInfoCommand command,
        CancellationToken cancellationToken = default)
    {
        var speciesId = SpeciesId.Create(command.SpeciesId);
        var breedId = BreedId.Create(command.BreedId);
        
        var breedResult = await _readDbContext.Breeds
            .FirstOrDefaultAsync(b => b.Id == breedId.Value, cancellationToken);
        if (breedResult == null)
            return Errors.General.NotFound(breedId.Value).ToErrorList();
        
        if(breedResult.SpeciesId != speciesId.Value)
            return Errors.General.NotFound(speciesId.Value).ToErrorList();
        
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var volunteerResult = await _volunteersRepository.GetById(
            VolunteerId.Create(command.VolunteerId),
            cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
            
        var petId = PetId.NewPetId();

        var name = PetName.Create(command.Name);
        var title = Title.Create(command.Title);
        var description = Description.Create(command.Description);
        var color = Color.Create(command.Color);
        var petHealthInformation = PetHealthInformation.Create(command.PetHealthInformation);
        
        var petAddress = Address.Create(
            command.Address.Region,
            command.Address.City,
            command.Address.Street,
            command.Address.Building,
            command.Address.Apartment);

        var phoneNumber = PhoneNumber.Create(command.PhoneNumber);

        var petSize = Size.Create(
            command.PetSizeDto.Weight,
            command.PetSizeDto.Height);

        var isNeutered = NeuteredStatus.Create(command.IsNeutered);
        var isVaccinated = RabiesVaccinationStatus.Create(command.IsVaccinated);
        var dateOfBirth = command.DateOfBirth;
        var status = command.Status;
        var dateOfCreation = command.DateOfCreation;


        var pet = Pet.Create(
            petId,
            volunteerResult.Value.Id,
            name.Value,
            speciesId,
            BreedId.Create(command.BreedId),
            [],
            title.Value,
            description.Value,
            color.Value,
            petHealthInformation.Value,
            petAddress.Value,
            phoneNumber.Value,
            petSize.Value,
            isNeutered.Value,
            isVaccinated.Value,
            dateOfBirth.Value,
            status,
            dateOfCreation);

        volunteerResult.Value.AddPet(pet.Value);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Created pet with id {PetId} from a volunteer with id {VolunteerId}", 
            petId, volunteerResult.Value.Id);
            
        return pet.Value.Id.Value;
    }
}