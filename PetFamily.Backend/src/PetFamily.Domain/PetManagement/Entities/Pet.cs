using CSharpFunctionalExtensions;
using PetFamily.Domain.PetManagement.PetVO;
using PetFamily.Domain.PetManagement.SharedVO;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SpesiesManagment.SpeciesVO;

namespace PetFamily.Domain.PetManagement.Entities;

public class Pet : SoftDeletableEntity<PetId>
{
    public PetName PetName { get; private set; }
    public SpeciesId SpeciesId { get; private set; }
    public BreedId BreedId { get; private set; }
    public ValueObjectList<PetPhoto> PetPhotos { get; private set; } = new ([]);
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
        PetName petName,
        SpeciesId speciesId,
        BreedId breedId,
        ValueObjectList<PetPhoto> petPhotos,
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
        PetName = petName;
        SpeciesId = speciesId;
        BreedId = breedId;
        PetPhotos = petPhotos;
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
        PetName petName,
        SpeciesId speciesId,
        BreedId breedId,
        ValueObjectList<PetPhoto> petPhotos,
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

    public void UpdatePetPhotos(ValueObjectList<PetPhoto> petPhotos)
    {
        PetPhotos = petPhotos;
    }
    
    public void SetPosition(Position position) => Position = position;
}
