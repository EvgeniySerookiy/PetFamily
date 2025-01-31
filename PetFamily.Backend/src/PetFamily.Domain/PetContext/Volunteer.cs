using CSharpFunctionalExtensions;
using PetFamily.Domain.SharedVO;
using PetFamily.Domain.VolunteerVO;

namespace PetFamily.Domain.PetContext;

public class Volunteer : Entity
{
    public Guid Id { get; private set; }
    public FullName FullName { get; private set; }
    public Email Email { get; private set; }
    public Description Description { get; private set; }
    public YearsOfExperience YearsOfExperience { get; private set; }
    public int PetsRehomed { get; private set; }
    public int PetsSeekingHome { get; private set; }
    public int PetsUnderTreatment { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public List<SocialNetwork> SocialNetworks { get; private set; }
    public AssistanceRequisites AssistanceRequisites { get; private set; }
    public List<Pet> Pets { get; private set; }

    private Volunteer(
        Guid id, 
        FullName fullName, 
        Email email,
        Description description,
        YearsOfExperience yearsOfExperience,
        PhoneNumber phoneNumber,
        List<SocialNetwork> socialNetworks,
        AssistanceRequisites assistanceRequisites,
        List<Pet> pets)
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
        SocialNetworks = socialNetworks;
        AssistanceRequisites = assistanceRequisites;
        Pets = pets;
    }

    public static Result<Volunteer> Create(
        Guid id,
        FullName fullName,
        Email email,
        Description description,
        YearsOfExperience yearsOfExperience,
        PhoneNumber phoneNumber,
        List<SocialNetwork> socialNetworks,
        AssistanceRequisites assistanceRequisites,
        List<Pet> pets)
    {
        var createFullName = FullName.Create(fullName.FirstName, fullName.LastName, fullName.MiddleName);
        if (createFullName.IsFailure)
        {
            return Result.Failure<Volunteer>(createFullName.Error);
        }
        
        var createEmail = Email.Create(email.Value);
        if (createEmail.IsFailure)
        {
            return Result.Failure<Volunteer>(createEmail.Error);
        }
        
        var createDescription = Description.Create(description.Value);
        if (createDescription.IsFailure)
        {
            return Result.Failure<Volunteer>(createDescription.Error);
        }
        
        var createYearsOfExperience = YearsOfExperience.Create(yearsOfExperience.Value);
        if (createYearsOfExperience.IsFailure)
        {
            return Result.Failure<Volunteer>(createYearsOfExperience.Error);
        }
        
        var createPhoneNumber = PhoneNumber.Create(phoneNumber.Value);
        if (createPhoneNumber.IsFailure)
        {
            return Result.Failure<Volunteer>(createPhoneNumber.Error);
        }
        
        var createAssistanceRequisites = AssistanceRequisites.Create(assistanceRequisites.Title, assistanceRequisites.Description);
        if (createAssistanceRequisites.IsFailure)
        {
            return Result.Failure<Volunteer>(createAssistanceRequisites.Error);
        }
        
        var volunteer = new Volunteer(
            id,
            createFullName.Value,
            createEmail.Value,
            createDescription.Value,
            createYearsOfExperience.Value,
            createPhoneNumber.Value,
            socialNetworks,
            assistanceRequisites,
            pets);
        
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