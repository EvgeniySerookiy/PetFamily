using CSharpFunctionalExtensions;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.SharedVO;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Domain.PetManagement.AggregateRoot;

public sealed class Volunteer : Shared.Entity<VolunteerId>
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
    public TransferSocialNetworkList TransferSocialNetworkList { get; private set; }
    public TransferRequisitesForHelpsList TransferRequisitesForHelpsList { get; private set; }
    public IReadOnlyList<Pet> Pets => _pets;
    
    private Volunteer(VolunteerId id) : base(id){}
    
    private Volunteer(
        VolunteerId id, 
        FullName fullName,
        Email email,
        Description description,
        YearsOfExperience yearsOfExperience,
        PhoneNumber phoneNumber,
        TransferRequisitesForHelpsList transferRequisitesForHelpsList,
        TransferSocialNetworkList transferSocialNetworkList) : base(id)
    {
        FullName = fullName;
        Email = email;
        Description = description;
        YearsOfExperience = yearsOfExperience;
        PhoneNumber = phoneNumber;
        TransferRequisitesForHelpsList = transferRequisitesForHelpsList;
        TransferSocialNetworkList = transferSocialNetworkList;
    }

    public static Result<Volunteer, Error> Create(
        VolunteerId id,
        FullName fullName,
        Email email,
        Description description,
        YearsOfExperience yearsOfExperience,
        PhoneNumber phoneNumber,
        TransferRequisitesForHelpsList transferRequisitesForHelpsList,
        TransferSocialNetworkList transferSocialNetworkList)
    {
        var volunteer = new Volunteer(
            id,
            fullName,
            email,
            description,
            yearsOfExperience,
            phoneNumber,
            transferRequisitesForHelpsList,
            transferSocialNetworkList);
        
        return volunteer;
    }

   private int CountPetsRehomed()
   {
       return _pets.Count(pet => pet.Status == AssistanceStatus.FoundHome);
   }
   
   private int CountPetsSeekingHome()
   {
       return _pets.Count(pet => pet.Status == AssistanceStatus.LookingForHome);
   }
   
   private int CountPetsUnderTreatment()
   {
       return _pets.Count(pet => pet.Status == AssistanceStatus.NeedsHelp);
   }
}