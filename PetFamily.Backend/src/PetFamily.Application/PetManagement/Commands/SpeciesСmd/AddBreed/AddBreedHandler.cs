using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SpeciesManagement.SpeciesVO;

namespace PetFamily.Application.PetManagement.Commands.SpeciesСmd.AddBreed;

public class AddBreedHandler : ICommandHandler<Guid, AddBreedCommand>
{
    private readonly ISpeciesWriteRepository _speciesWriteRepository;
    private readonly IValidator<AddBreedCommand> _validator;
    private readonly ILogger<AddBreedHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public AddBreedHandler(
        ISpeciesWriteRepository speciesWriteRepository,
        IValidator<AddBreedCommand> validator,
        ILogger<AddBreedHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _speciesWriteRepository = speciesWriteRepository;
        _validator = validator;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        AddBreedCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var speciesResult = await _speciesWriteRepository.GetById(
            SpeciesId.Create(command.Id),
            cancellationToken);
        if (speciesResult.IsFailure)
            return speciesResult.Error.ToErrorList();

        var breedId = BreedId.NewBreedId();

        var name = BreedName.Create(command.Name);

        var breedExists = speciesResult.Value.EnsureBreedDoesNotExist(name.Value);

        if (breedExists.IsFailure)
        {
            _logger.LogWarning("Breed with name {BreedName} already exists in species id: {SpeciesId}", command.Name,
                command.Id);
            return Errors.Breed.AlreadyExist().ToErrorList();
        }

        var breed = Domain.SpeciesManagement.Entities.Breed.Create(breedId, name.Value);

        speciesResult.Value.AddBreed(breed.Value);

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Created breed with id: {BreedId} from a species with id: {SpeciesId}",
            breedId, command.Id);

        return breed.Value.Id.Value;
    }
}