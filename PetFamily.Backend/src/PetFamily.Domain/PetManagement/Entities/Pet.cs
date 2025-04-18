using CSharpFunctionalExtensions;
using PetFamily.Domain.PetManagement.PetVO;
using PetFamily.Domain.PetManagement.SharedVO;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SpeciesManagement.SpeciesVO;

namespace PetFamily.Domain.PetManagement.Entities;

public class Pet : SoftDeletableEntity<PetId>
{
    private List<PetPhoto> _petPhotos = new();
    public PetName PetName { get; private set; }
    public VolunteerId VolunteerId { get; private set; }
    public SpeciesId SpeciesId { get; private set; }
    public BreedId BreedId { get; private set; }
    public IReadOnlyList<PetPhoto> PetPhotos => _petPhotos;
    public Title Title { get; private set; }
    public Description Description { get; private set; }
    public Position Position { get; private set; }
    public Color Color { get; private set; }
    public PetHealthInformation PetHealthInformation { get; private set; }
    public Address PetAddress { get; private set; }
    public PhoneNumber OwnerPhoneNumber { get; private set; }
    public Size Size { get; private set; }
    public NeuteredStatus IsNeutered { get; private set; }
    public RabiesVaccinationStatus IsVaccinated { get; private set; }
    public DateTime? DateOfBirth { get; private set; }
    public AssistanceStatus Status { get; private set; }
    public DateTime DateOfCreation { get; private set; }

    private Pet(PetId id) : base(id) { }

    private Pet(
        PetId id,
        VolunteerId volunteerId,
        PetName petName,
        SpeciesId speciesId,
        BreedId breedId,
        List<PetPhoto> petPhotos,
        Title title,
        Description description,
        Color color,
        PetHealthInformation petHealthInformation,
        Address petAddress,
        PhoneNumber ownerPhoneNumber,
        Size size,
        NeuteredStatus isNeutered,
        RabiesVaccinationStatus isVaccinated,
        DateTime? dateOfBirth,
        AssistanceStatus status,
        DateTime dateOfCreation) : base(id)
    {
        VolunteerId = volunteerId;
        PetName = petName;
        SpeciesId = speciesId;
        BreedId = breedId;
        _petPhotos = petPhotos;
        Title = title;
        Description = description;
        Color = color;
        PetHealthInformation = petHealthInformation;
        PetAddress = petAddress;
        OwnerPhoneNumber = ownerPhoneNumber;
        Size = size;
        IsNeutered = isNeutered;
        IsVaccinated = isVaccinated;
        DateOfBirth = dateOfBirth;
        Status = status;
        DateOfCreation = dateOfCreation;
    }

    public static Result<Pet, Error> Create(
        PetId id,
        VolunteerId volunteerId,
        PetName petName,
        SpeciesId speciesId,
        BreedId breedId,
        List<PetPhoto> petPhotos,
        Title title,
        Description description,
        Color color,
        PetHealthInformation petHealthInformation,
        Address petAddress,
        PhoneNumber ownerPhoneNumber,
        Size size,
        NeuteredStatus isNeutered,
        RabiesVaccinationStatus isVaccinated,
        DateTime dateOfBirth,
        AssistanceStatus status,
        DateTime dateOfCreation)
    {
        var pet = new Pet(
            id,
            volunteerId,
            petName,
            speciesId,
            breedId,
            petPhotos,
            title,
            description,
            color,
            petHealthInformation,
            petAddress,
            ownerPhoneNumber,
            size,
            isNeutered,
            isVaccinated,
            dateOfBirth,
            status,
            dateOfCreation); 
        
        return pet;
    }

    public void UpdatePet(
        PetName name,
        SpeciesId speciesId,
        BreedId breedId,
        Title title,
        Description description,
        Color color,
        PetHealthInformation petHealthInformation,
        Address petAddress,
        PhoneNumber ownerPhoneNumber,
        Size size,
        NeuteredStatus isNeutered,
        RabiesVaccinationStatus isVaccinated,
        DateTime dateOfBirth,
        AssistanceStatus status,
        DateTime dateOfCreation)
    {
        PetName = name;
        SpeciesId = speciesId;
        BreedId = breedId;
        Title = title;
        Description = description;
        Color = color;
        PetHealthInformation = petHealthInformation;
        PetAddress = petAddress;
        OwnerPhoneNumber = ownerPhoneNumber;
        Size = size;
        IsNeutered = isNeutered;
        IsVaccinated = isVaccinated;
        DateOfBirth = dateOfBirth;
        Status = status;
        DateOfCreation = dateOfCreation;
    }

    public void UpdatePetStatus(AssistanceStatus status)
    {
        Status = status;
    }

    public UnitResult<Error> SetMainPhoto(string photoPath)
    {
        var petPhoto = PetPhoto.Create(PhotoPath.Create(photoPath).Value);
        if(_petPhotos.Contains(petPhoto.Value) == false)
            return UnitResult.Failure(Errors.General.NotFound());

        if (_petPhotos[0] == petPhoto.Value)
            return UnitResult.Success<Error>();
        
        _petPhotos.Remove(petPhoto.Value);
        _petPhotos.Insert(0, petPhoto.Value);
        
        return UnitResult.Success<Error>();
    }

    public void UpdatePetPhotos(IEnumerable<PetPhoto> petPhotos)
    {
        _petPhotos = petPhotos.ToList();
    }

    public void RemoveAll(List<PetPhoto> petPhotosToRemove)
    {
        _petPhotos.RemoveAll(petPhotosToRemove.Contains);
    }
    
    public void SetPosition(Position position) => Position = position;
}
