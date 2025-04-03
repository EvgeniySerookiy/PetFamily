using CSharpFunctionalExtensions;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.PetVO;
using PetFamily.Domain.PetManagement.SharedVO;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Domain.PetManagement.AggregateRoot;

public sealed class  Volunteer : SoftDeletableEntity<VolunteerId>
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

    private Volunteer(VolunteerId id) : base(id) {}

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

    public void UpdateMainInfo(
        FullName fullName,
        Email email,
        Description description,
        YearsOfExperience yearsOfExperience,
        PhoneNumber phoneNumber)
    {
        FullName = fullName;
        Email = email;
        Description = description;
        YearsOfExperience = yearsOfExperience;
        PhoneNumber = phoneNumber;
    }

    public void UpdateRequisitesForHelp(
        IEnumerable<RequisitesForHelp> requisitesForHelpsList)
    {
        TransferRequisitesForHelpsList = TransferRequisitesForHelpsList.Create(requisitesForHelpsList).Value;
    }

    public void UpdateSocialNetworkList(
        IEnumerable<SocialNetwork> socialNetworkList)
    {
        TransferSocialNetworkList = TransferSocialNetworkList.Create(socialNetworkList).Value;
    }

    public override void Delete()
    {
        base.Delete();

        foreach (var pet in _pets)
        {
            pet.Delete();
        }
    }

    public override void Restore()
    {
        base.Restore();

        foreach (var pet in _pets)
        {
            pet.Restore();
        }
    }

    public UnitResult<Error> AddPet(Pet pet)
    {
        var serialNumberResult = Position.Create(_pets.Count + 1);
        if (serialNumberResult.IsFailure)
            return serialNumberResult.Error;

        pet.SetPosition(serialNumberResult.Value);

        _pets.Add(pet);
        return Result.Success<Error>();
    }

    private void ShiftPositions(int currentPosition, int nextPosition)
    {
        var petsToUpdate = Pets.Where(p =>
            (currentPosition > nextPosition && p.Position.Value >= nextPosition &&
             p.Position.Value < currentPosition) ||
            (currentPosition < nextPosition && p.Position.Value <= nextPosition &&
             p.Position.Value > currentPosition)
        ).ToList();

        foreach (var p in petsToUpdate)
        {
            var newPosition =
                Position.Create(
                    p.Position.Value + (currentPosition > nextPosition ? 1 : -1));
            p.SetPosition(newPosition.Value);
        }
    }
    
    public UnitResult<Error> MovePet(Pet pet, int nextPosition)
    {
        if (nextPosition > Pets.Count || nextPosition < 1)
            return UnitResult.Failure(Errors.General.OutOfRange(nextPosition));

        var result = Pets.FirstOrDefault(p => p.Id == pet.Id);
        if (result == null)
            return Errors.General.NotFound(pet.Id);

        var currentPosition = pet.Position.Value;

        if (currentPosition == nextPosition)
            return Result.Success<Error>();
        
        ShiftPositions(currentPosition, nextPosition);

        var position = Position.Create(nextPosition);
        pet.SetPosition(position.Value);
        return Result.Success<Error>();
    }

    public Result<Pet, Error> GetPetById(Guid petId)
    {
        var pet = Pets.FirstOrDefault(p => p.Id == petId);
        if (pet == null)
            return Errors.General.NotFound(petId);

        return pet;
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