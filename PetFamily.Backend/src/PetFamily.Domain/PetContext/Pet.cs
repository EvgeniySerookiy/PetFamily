using CSharpFunctionalExtensions;
using PetFamily.Domain.PetVO;
using PetFamily.Domain.SharedVO;
using PetFamily.Domain.SpeciesContext.SpeciesVO;

namespace PetFamily.Domain.PetContext;

public class Pet : Entity
{
    public Guid Id { get; private set; }
    public Name Name { get; private set; }
    public SpeciesId SpeciesId { get; private set; }
    public BreedId BreedId { get; private set; }
    public Title Title { get; private set; }
    public Description Description { get; private set; }
    public Color Color { get; private set; }
    public PetHealthInformation PetHealthInformation { get; private set; }
    public Address PetAddress { get; private set; }
    public PhoneNumber OwnerPhoneNumber { get; private set; }
    public Size Size { get; private set; }
    public NeuteredStatus IsNeutered { get; private set; }
    public RabiesVaccinationStatus IsVaccinated { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public AssistanceStatus Status { get; private set; }
    public List<RequisitesForHelp> RequisitesForHelps { get; private set; }
    public DateTime DateOfCreation { get; private set; }

    private Pet(
        Guid id,
        Name name,
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
        List<RequisitesForHelp> requisitesForHelps,
        DateTime dateOfCreation)
    {
        Id = id;
        Name = name;
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
        RequisitesForHelps = requisitesForHelps;
        DateOfCreation = dateOfCreation;
    }

    public static Result<Pet> Create(
        Guid id,
        Name name,
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
        List<RequisitesForHelp> requisitesForHelps,
        DateTime dateOfCreation)
    {
        var createName = Name.Create(name.Value);
        if (createName.IsFailure)
        {
            return Result.Failure<Pet>(createName.Error);
        }
        
        var createSpeciesId = SpeciesId.Create(speciesId.Value);
        
        var createBreedId = BreedId.Create(breedId.Value);
        
        var createTitle = Title.Create(title.Value);
        if (createTitle.IsFailure)
        {
            return Result.Failure<Pet>(createTitle.Error);
        }
        
        var createDescription = Description.Create(description.Value);
        if (createDescription.IsFailure)
        {
            return Result.Failure<Pet>(createDescription.Error);
        }
        
        var createColor = Color.Create(color.Value);
        if (createColor.IsFailure)
        {
            return Result.Failure<Pet>(createColor.Error);
        }
        
        var createPetHealthInformation = PetHealthInformation.Create(petHealthInformation.Value);
        if (createPetHealthInformation.IsFailure)
        {
            return Result.Failure<Pet>(createPetHealthInformation.Error);
        }
        
        var createPetAddress = Address.Create(petAddress.Region, petAddress.City, petAddress.Street, petAddress.Building, petAddress.Apartment);
        if (createPetAddress.IsFailure)
        {
            return Result.Failure<Pet>(createPetAddress.Error);
        }
        
        var createOwnerPhoneNumber = PhoneNumber.Create(ownerPhoneNumber.Value);
        if (createOwnerPhoneNumber.IsFailure)
        {
            return Result.Failure<Pet>(createOwnerPhoneNumber.Error);
        }
        
        var createSize = Size.Create(size.Weight, size.Height);
        if (createSize.IsFailure)
        {
            return Result.Failure<Pet>(createSize.Error);
        }
        
        var createNeuteredStatus = NeuteredStatus.Create(isNeutered.Value);
        
        var createIsVaccinated = RabiesVaccinationStatus.Create(isVaccinated.Value);
        
        var pet = new Pet(
            id,
            createName.Value,
            createSpeciesId,
            createBreedId,
            createTitle.Value,
            createDescription.Value,
            createColor.Value,
            createPetHealthInformation.Value,
            createPetAddress.Value,
            createOwnerPhoneNumber.Value,
            createSize.Value,
            createNeuteredStatus.Value,
            createIsVaccinated.Value,
            dateOfBirth,
            status,
            requisitesForHelps,
            dateOfCreation);
        
        return Result.Success(pet);
    }
}