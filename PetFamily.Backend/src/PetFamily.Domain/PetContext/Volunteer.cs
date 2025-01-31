using CSharpFunctionalExtensions;
using PetFamily.Domain.SharedVO;
using PetFamily.Domain.VolunteerVO;

namespace PetFamily.Domain.PetContext;

public class Volunteer : Entity
{
    private readonly List<SocialNetwork> _socialNetworks = new();
    private readonly List<Pet> _pets = new();
    public Guid Id { get; private set; }
    public FullName FullName { get; private set; }
    public Email Email { get; private set; }
    public Description Description { get; private set; }
    public YearsOfExperience YearsOfExperience { get; private set; }
    public int PetsRehomed { get; private set; }
    public int PetsSeekingHome { get; private set; }
    public int PetsUnderTreatment { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;
    public AssistanceRequisites AssistanceRequisites { get; private set; }
    public IReadOnlyList<Pet> Pets => _pets;

    private Volunteer(
        Guid id, 
        FullName fullName, 
        Email email,
        Description description,
        YearsOfExperience yearsOfExperience,
        PhoneNumber phoneNumber,
        AssistanceRequisites assistanceRequisites)
    {
        Id = id;
        FullName = fullName;
        Email = email;
        Description = description;
        YearsOfExperience = yearsOfExperience;
        PetsRehomed = CountPetsRehomed();
        PetsSeekingHome = CountPetsSeekingHome();
        PetsUnderTreatment = CountPetsUnderTreatment();
        PhoneNumber = phoneNumber;
        AssistanceRequisites = assistanceRequisites;
    }

    public void AddSocialNetwork(SocialNetwork socialNetwork)
    {
        _socialNetworks.Add(socialNetwork);
    }

    public void AddPet(Pet pet)
    {
        _pets.Add(pet);
    }

    public static Result<Volunteer> Create(
        Guid id,
        FullName fullName,
        Email email,
        Description description,
        YearsOfExperience yearsOfExperience,
        PhoneNumber phoneNumber,
        AssistanceRequisites assistanceRequisites)
    {
        var volunteer = new Volunteer(
            id,
            fullName,
            email,
            description,
            yearsOfExperience,
            phoneNumber,
            assistanceRequisites);
        
        return Result.Success(volunteer);
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