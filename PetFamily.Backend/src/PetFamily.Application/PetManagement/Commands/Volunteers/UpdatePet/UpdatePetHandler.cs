using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Domain.PetManagement.PetVO;
using PetFamily.Domain.PetManagement.SharedVO;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SpeciesManagement.SpeciesVO;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.UpdatePet;

public class UpdatePetHandler : ICommandHandler<Guid, UpdatePetCommand>
{
    private readonly IVolunteersWriteRepository _volunteersWriteRepository;
    private readonly IReadDbContext _readDbContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdatePetHandler> _logger;

    public UpdatePetHandler(
        IVolunteersWriteRepository volunteersWriteRepository,
        IReadDbContext readDbContext,
        IUnitOfWork unitOfWork,
        ILogger<UpdatePetHandler> logger)
    {
        _volunteersWriteRepository = volunteersWriteRepository;
        _readDbContext = readDbContext;
        _unitOfWork = unitOfWork;
        _logger = logger;
        
    }
    public async Task<Result<Guid, ErrorList>> Handle(
        UpdatePetCommand command, 
        CancellationToken cancellationToken = default)
    {
        var speciesQuery = await _readDbContext.Species
            .FirstOrDefaultAsync(b => b.Id == command.SpeciesId, cancellationToken);
        if (speciesQuery is null)
            return Errors.Species.NotFound(command.SpeciesId).ToErrorList();
        
        var breedQuery = await _readDbContext.Breeds
            .FirstOrDefaultAsync(b => b.Id == command.BreedId, cancellationToken);
        if (breedQuery is null)
            return Errors.Breed.NotFound(command.BreedId).ToErrorList();

        var volunteerResult = await _volunteersWriteRepository.GetById(command.VolunteerId);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var petResult = volunteerResult.Value.GetByPetId(command.PetId);
        if (petResult.IsFailure)
            return petResult.Error.ToErrorList();
        
        var petName = PetName.Create(command.Name);
        var speciesId = SpeciesId.Create(command.SpeciesId);
        var breedId = BreedId.Create(command.BreedId);
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
        
        petResult.Value.UpdatePet(
            petName.Value,
            speciesId,
            breedId,
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
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation(
            "Updated pet with id: {PetId} from a volunteer with id: {VolunteerId}", 
            command.PetId, 
            command.VolunteerId);

        return petResult.Value.Id.Value;
    }
}