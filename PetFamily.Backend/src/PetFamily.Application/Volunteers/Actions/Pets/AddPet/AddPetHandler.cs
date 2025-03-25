using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.PetVO;
using PetFamily.Domain.PetManagement.SharedVO;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SpesiesManagment.SpeciesVO;

namespace PetFamily.Application.Volunteers.Actions.Pets.AddPet;

public class AddPetHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<AddPetHandler> _logger;

    public AddPetHandler(
        IUnitOfWork unitOfWork,
        IVolunteersRepository volunteersRepository,
        ILogger<AddPetHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        AddPetCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersRepository.GetById(
            VolunteerId.Create(command.VolunteerId),
            cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error;
            
        var petId = PetId.NewPetId();

        var name = PetName.Create(command.MainPetInfo.Name);
        var title = Title.Create(command.MainPetInfo.Title);
        var description = Description.Create(command.MainPetInfo.Description);
        var color = Color.Create(command.MainPetInfo.Color);
        var petHealthInformation = PetHealthInformation.Create(command.MainPetInfo.PetHealthInformation);
        
        var petAddress = Address.Create(
            command.MainPetInfo.Address.Region,
            command.MainPetInfo.Address.City,
            command.MainPetInfo.Address.Street,
            command.MainPetInfo.Address.Building,
            command.MainPetInfo.Address.Apartment);

        var phoneNumber = PhoneNumber.Create(command.MainPetInfo.PhoneNumber);

        var petSize = Size.Create(
            command.MainPetInfo.PetSize.Weight,
            command.MainPetInfo.PetSize.Height);

        var isNeutered = NeuteredStatus.Create(command.MainPetInfo.IsNeutered);
        var isVaccinated = RabiesVaccinationStatus.Create(command.MainPetInfo.IsVaccinated);
        var dateOfBirth = command.MainPetInfo.DateOfBirth;
        var status = command.MainPetInfo.Status;
        var dateOfCreation = command.MainPetInfo.DateOfCreation;


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