using PetFamily.Domain.PetContext;
using PetFamily.Domain.Shared;
using PetFamily.Domain.SharedVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO;

namespace PetFamily.Domain.VolunteerContext;

public sealed class Volunteer : Entity<VolunteerId>
{
    private readonly List<Pet> _pets = new();
    public FullName FullName { get; private set; }
    public Email Email { get; private set; }
    public Description Description { get; private set; }
    public YearsOfExperience YearsOfExperience { get; private set; }
    public int PetsRehomed { get; private set; }
    public int PetsSeekingHome { get; private set; }
    public int PetsUnderTreatment { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public AssistanceRequisites AssistanceRequisites { get; private set; }
    public TransferSocialNetworkList TransferSocialNetworkList { get; private set; }
    public IReadOnlyList<Pet> Pets => _pets;
    
    private Volunteer(VolunteerId id) : base(id){}
    private Volunteer(
        VolunteerId id, 
        FullName fullName,
        Email email,
        Description description,
        YearsOfExperience yearsOfExperience,
        PhoneNumber phoneNumber,
        AssistanceRequisites assistanceRequisites,
        TransferSocialNetworkList transferSocialNetworkList) : base(id)
    {
        FullName = fullName;
        Email = email;
        Description = description;
        YearsOfExperience = yearsOfExperience;
        PhoneNumber = phoneNumber;
        AssistanceRequisites = assistanceRequisites;
        TransferSocialNetworkList = transferSocialNetworkList;
    }

    public static Result<Volunteer> Create(
        VolunteerId id,
        FullName fullName,
        Email email,
        Description description,
        YearsOfExperience yearsOfExperience,
        PhoneNumber phoneNumber,
        AssistanceRequisites assistanceRequisites,
        TransferSocialNetworkList transferSocialNetworkList)
    {
        var volunteer = new Volunteer(
            id,
            fullName,
            email,
            description,
            yearsOfExperience,
            phoneNumber,
            assistanceRequisites,
            transferSocialNetworkList);
        
        return volunteer;
    }

   private int CountPetsRehomed()
   {
       return Pets.Count(pet => pet.Status == AssistanceStatus.FoundHome);
   }
   
   private int CountPetsSeekingHome()
   {
       return Pets.Count(pet => pet.Status == AssistanceStatus.LookingForHome);
   }
   
   private int CountPetsUnderTreatment()
   {
       return Pets.Count(pet => pet.Status == AssistanceStatus.NeedsHelp);
   }
}