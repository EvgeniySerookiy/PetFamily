using CSharpFunctionalExtensions;
using PetFamily.Application.Database;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.PetVO;
using PetFamily.Domain.PetManagement.SharedVO;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SpesiesManagment.SpeciesVO;

namespace PetFamily.Application.Volunteers.Actions.Pets.AddPet;

public class AddPetHandler
{
    private const string BUCKET_NAME = "photos";
    
    private readonly IFileProvider _fileProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteersRepository _volunteersRepository;

    public AddPetHandler(
        IFileProvider fileProvider,
        IUnitOfWork unitOfWork,
        IVolunteersRepository volunteersRepository)
    {
        _fileProvider = fileProvider;
        _unitOfWork = unitOfWork;
        _volunteersRepository = volunteersRepository;
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
        
        List<FileContent> fileContents = [];
        List<PhotoPath> photoPaths = [];
        foreach (var file in command.CollectionFiles.Files)
        {
            // var extension = Path.GetExtension(file.FileName);
            var fullFileName = Path.GetFileName(file.FileName);
            

            var guid = Guid.NewGuid();
            
            var photoPath = PhotoPath.Create(guid, fullFileName);
            if(photoPath.IsFailure)
                return photoPath.Error;
            
            photoPaths.Add(photoPath.Value);
            
            var fileContent = new FileContent(file.Content, guid + "." + fullFileName);
            fileContents.Add(fileContent);
        }

        var fileData = new FileData(fileContents, BUCKET_NAME);
        
        var uploadResult = await _fileProvider.UploadFiles(fileData, cancellationToken);
        if (uploadResult.IsFailure)
            return uploadResult.Error;

        var petPhotos = photoPaths.Select(f => PetPhoto.Create(f).Value);
        
        var transferFilesList = TransferFilesList.Create(petPhotos);

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
            transferFilesList.Value,
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

        return pet.Value.Id.Value;
    }
}