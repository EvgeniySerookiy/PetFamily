using PetFamily.Domain.PetContext.PetVO;
using PetFamily.Domain.Shared;
using PetFamily.Domain.SharedVO;
using PetFamily.Domain.SpeciesContext.SpeciesVO;

namespace PetFamily.Domain.PetContext;

public class Pet : Entity<PetId>
{
    public NotEmptyString Name { get; private set; }
    //public SpeciesId SpeciesId { get; private set; }
    //public BreedId BreedId { get; private set; }
    public string Title { get; private set; }
    //public Description Description { get; private set; }
    //public NotEmptyString Color { get; private set; }
    //public PetHealthInformation PetHealthInformation { get; private set; }
    //public Address PetAddress { get; private set; }
    //public PhoneNumber OwnerPhoneNumber { get; private set; }
    //public Size Size { get; private set; }
    //public NeuteredStatus IsNeutered { get; private set; }
    //public RabiesVaccinationStatus IsVaccinated { get; private set; }
    //public DateTime? DateOfBirth { get; private set; }
    //public AssistanceStatus Status { get; private set; }
    //public TransferRequisitesForHelpsList TransferRequisitesForHelpsList { get; private set; }
    //public DateTime DateOfCreation { get; private set; }

    private Pet(PetId id) : base(id) { }

    private Pet(PetId id, string title, NotEmptyString name) : base(id)
    {
        Title = title;
        Name = name;
    }

    public static Result<Pet> Create(PetId id, string title, NotEmptyString name)
    {
        // Удалены поля из метода Create, так как они больше не нужны
        var pet = new Pet(id, title, name); 
        return pet;
    }
}
