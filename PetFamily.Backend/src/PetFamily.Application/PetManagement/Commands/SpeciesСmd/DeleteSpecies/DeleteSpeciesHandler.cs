using CSharpFunctionalExtensions;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SpeciesManagement.SpeciesVO;

namespace PetFamily.Application.PetManagement.Commands.SpeciesСmd.DeleteSpecies;

public class DeleteSpeciesHandler : ICommandHandler<Guid, DeleteSpeciesCommand>
{
    private readonly IVolunteersReadRepository _volunteersReadRepository;
    private readonly ISpeciesWriteRepository _speciesWriteRepository;
    
    public DeleteSpeciesHandler(
        IVolunteersReadRepository volunteersReadRepository,
        ISpeciesWriteRepository speciesWriteRepository)
    {
        _volunteersReadRepository = volunteersReadRepository;
        _speciesWriteRepository = speciesWriteRepository;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteSpeciesCommand command, 
        CancellationToken cancellationToken = default)
    {
        var petResult = await _volunteersReadRepository
            .IsSpeciesUsedByAnyPet(command.SpeciesId, cancellationToken);
        if (petResult.IsFailure)
            petResult.Error.ToErrorList();
        
        await _speciesWriteRepository.Delete(
            SpeciesId.Create(command.SpeciesId), 
            cancellationToken);
        
        return command.SpeciesId;
    }
}