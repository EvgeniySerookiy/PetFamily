using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SpeciesManagement.SpeciesVO;

namespace PetFamily.Application.PetManagement.Commands.SpeciesСmd.AddBreed;

public class AddBreedHandler : ICommandHandler<Guid, AddBreedCommand>
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IValidator<AddBreedCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public AddBreedHandler(
        ISpeciesRepository speciesRepository,
        IValidator<AddBreedCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _speciesRepository = speciesRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<Guid, ErrorList>> Handle(
        AddBreedCommand command, 
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var speciesResult = await _speciesRepository.GetById(
            SpeciesId.Create(command.Id), 
            cancellationToken);
        if(speciesResult.IsFailure)
            return speciesResult.Error.ToErrorList();

        var breedId = BreedId.NewBreedId();
        
        var name = BreedName.Create(command.Name);
        
        var breedNameResult = await _speciesRepository.GetByBreedName(
            speciesResult.Value.Id, 
            name.Value, 
            cancellationToken);
        if(breedNameResult.IsFailure)
            return breedNameResult.Error.ToErrorList();
        
        var breed = Domain.SpeciesManagement.Entities.Breed.Create(breedId, name.Value);
        
        speciesResult.Value.AddBreed(breed.Value);
        
        await _unitOfWork.SaveChanges(cancellationToken);

        return breed.Value.Id.Value;
    }
}