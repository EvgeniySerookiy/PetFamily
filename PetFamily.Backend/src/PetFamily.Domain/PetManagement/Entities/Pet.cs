using CSharpFunctionalExtensions;
using PetFamily.Domain.PetManagement.PetVO;
using PetFamily.Domain.PetManagement.SharedVO;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared;
using PetFamily.Domain.SpesiesManagment.SpeciesVO;

namespace PetFamily.Domain.PetManagement.Entities;

public class Pet : Shared.Entity<PetId>
{
    private bool _isDeleted;
    public PetName PetName { get; private set; }
    public SpeciesId SpeciesId { get; private set; }
    public BreedId BreedId { get; private set; }
    public Title Title { get; private set; }
    public Description Description { get; private set; }
    public Color Color { get; private set; }
    public PetHealthInformation PetHealthInformation { get; private set; }
    public Address PetAddress { get; private set; }
    public PhoneNumber OwnerPhoneNumber { get; private set; }
    
    // переделать с int в double Size
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
        DateTime dateOfCreation) : base(id)
    {
        PetName = petName;
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

    public void Delete()
    {
        _isDeleted = true;
    }

    public void Restore()
    {
        _isDeleted = false;
    }

    public static Result<Pet> Create(
        PetId id,
        PetName petName,
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
        TransferRequisitesForHelpsList transferRequisitesForHelpsList,
        DateTime dateOfCreation)
    {
        var pet = new Pet(
            id,
            petName,
            speciesId,
            breedId,
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
}
