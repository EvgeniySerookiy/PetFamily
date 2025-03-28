using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.PetVO;
using PetFamily.Domain.PetManagement.SharedVO;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SpesiesManagment.SpeciesVO;

namespace PetFamily.Application.Volunteers.Actions.Pets.AddPet;

public class AddPetHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<AddPetHandler> _logger;
    private readonly IValidator<MainPetInfoCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public AddPetHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<AddPetHandler> logger,
        IValidator<MainPetInfoCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        Guid id,
        MainPetInfoCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var volunteerResult = await _volunteersRepository.GetById(
            VolunteerId.Create(id),
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
            name.Value,
            SpeciesId.NewSpeciesId(),
            BreedId.NewBreedId(),
            new ValueObjectList<PetPhoto>([]),
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