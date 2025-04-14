using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SpeciesManagement.SpeciesVO;

namespace PetFamily.Application.PetManagement.Commands.SpeciesСmd.DeleteSpecies;

public class DeleteSpeciesHandler : ICommandHandler<Guid, DeleteSpeciesCommand>
{
    private readonly IReadDbContext _readDbContext;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteSpeciesHandler(
        IReadDbContext readDbContext,
        ISpeciesRepository speciesRepository,
        IUnitOfWork unitOfWork)
    {
        _readDbContext = readDbContext;
        _speciesRepository = speciesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteSpeciesCommand command, 
        CancellationToken cancellationToken = default)
    {
        var petResult = await _readDbContext.Pets
            .FirstOrDefaultAsync(p => p.SpeciesId == command.SpeciesId, cancellationToken);

        if (petResult != null)
            return Errors.Species.IsCurrentlyUsed().ToErrorList();
        
        await _speciesRepository.DeleteSpecies(
            SpeciesId.Create(command.SpeciesId), 
            cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        return command.SpeciesId;
    }
}