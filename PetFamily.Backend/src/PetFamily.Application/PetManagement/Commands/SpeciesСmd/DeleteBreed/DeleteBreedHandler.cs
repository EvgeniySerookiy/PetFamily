using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SpeciesManagement.SpeciesVO;

namespace PetFamily.Application.PetManagement.Commands.SpeciesСmd.DeleteBreed;

public class DeleteBreedHandler : ICommandHandler<Guid, DeleteBreedCommand>
{
    private readonly ISpeciesWriteRepository _speciesWriteRepository;
    private readonly IVolunteersReadRepository _volunteersReadRepository;
    private readonly ILogger<DeleteBreedHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBreedHandler(
        ISpeciesWriteRepository speciesWriteRepository,
        IVolunteersReadRepository volunteersReadRepository,
        ILogger<DeleteBreedHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _speciesWriteRepository = speciesWriteRepository;
        _volunteersReadRepository = volunteersReadRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteBreedCommand command, 
        CancellationToken cancellationToken = default)
    {
        var petResult = await _volunteersReadRepository.
            IsBreedUsedByAnyPet(command.SpeciesId, command.BreedId, cancellationToken);
        if (petResult.IsFailure)
            petResult.Error.ToErrorList();
        
        var speciesResult = await _speciesWriteRepository.GetById(
            SpeciesId.Create(command.SpeciesId),
            cancellationToken);
        if (speciesResult.IsFailure)
            return speciesResult.Error.ToErrorList();
        
        var breedResult = speciesResult.Value.DeleteByBreedId(BreedId.Create(command.BreedId));
        if (breedResult.IsFailure)
            return breedResult.Error.ToErrorList();
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Deleted breed with id: {BreedId} from a species with id: {SpeciesId}",
            command.BreedId, command.SpeciesId);

        return command.BreedId;
    }
}