using System.Drawing;
using PetFamily.Domain.PetContext.PetVO;
using PetFamily.Domain.Shared;
using PetFamily.Domain.SharedVO;
using PetFamily.Domain.SpeciesContext.SpeciesVO;
using Size = PetFamily.Domain.PetContext.PetVO.Size;

namespace PetFamily.Domain.PetContext;

public class Pet : Entity<PetId>
{
    public NotEmptyString Name { get; private set; }
    public SpeciesId SpeciesId { get; private set; }
    public BreedId BreedId { get; private set; }
    public Title Title { get; private set; }
    public Description Description { get; private set; }
    public NotEmptyString Color { get; private set; }
    public PetHealthInformation PetHealthInformation { get; private set; }
    public Address PetAddress { get; private set; }
    public PhoneNumber OwnerPhoneNumber { get; private set; }
    public Size Size { get; private set; }
    public NeuteredStatus IsNeutered { get; private set; }
    public RabiesVaccinationStatus IsVaccinated { get; private set; }
    public DateTime? DateOfBirth { get; private set; }
    public AssistanceStatus Status { get; private set; }
    public TransferRequisitesForHelpsList TransferRequisitesForHelpsList { get; private set; }
    public DateTime DateOfCreation { get; private set; }

    private Pet(PetId id) : base(id) { }

    private Pet(
        PetId id, 
        NotEmptyString name,
        SpeciesId speciesId,
        BreedId breedId,
        Title title,
        Description description,
        NotEmptyString color,
        PetHealthInformation petHealthInformation,
        Address petAddress,
        PhoneNumber ownerPhoneNumber,
        Size size,
        NeuteredStatus isNeutered,
        RabiesVaccinationStatus isVaccinated,
        DateTime dateOfBirth,
        AssistanceStatus status,
        TransferRequisitesForHelpsList transferRequisitesForHelpsList,
        DateTime dateOfCreation) : base(id)
    {
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
        TransferRequisitesForHelpsList = transferRequisitesForHelpsList;
        DateOfCreation = dateOfCreation;
    }

    public static Result<Pet> Create(
        PetId id,
        NotEmptyString name,
        SpeciesId speciesId,
        BreedId breedId,
        Title title,
        Description description,
        NotEmptyString color,
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
            transferRequisitesForHelpsList,
            dateOfCreation); 
        
        return pet;
    }
}
