using CSharpFunctionalExtensions;
using PetFamily.Domain.PetVO;
using PetFamily.Domain.SharedVO;
using PetFamily.Domain.SpeciesContext.SpeciesVO;

namespace PetFamily.Domain.PetContext;

public class Pet : Entity
{
    private readonly List<RequisitesForHelp> _requisitesForHelps = new();
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
    public IReadOnlyList<RequisitesForHelp> RequisitesForHelps => _requisitesForHelps;
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
        DateOfCreation = dateOfCreation;
    }

    public void AddRequisitesForHelp(RequisitesForHelp requisitesForHelp)
    {
        _requisitesForHelps.Add(requisitesForHelp);
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
        DateTime dateOfCreation)
    {
        var pet = new Pet(
            id,
            name,
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
        
        return Result.Success(pet);
    }
}