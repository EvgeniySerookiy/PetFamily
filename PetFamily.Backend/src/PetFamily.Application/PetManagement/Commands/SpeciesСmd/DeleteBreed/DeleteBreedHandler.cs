using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SpeciesManagement.SpeciesVO;

namespace PetFamily.Application.PetManagement.Commands.SpeciesСmd.DeleteBreed;

public class DeleteBreedHandler : ICommandHandler<Guid, DeleteBreedCommand>
{
    private readonly IReadDbContext _readDbContext;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBreedHandler(
        IReadDbContext readDbContext, 
        ISpeciesRepository speciesRepository,
        IUnitOfWork unitOfWork)
    {
        _readDbContext = readDbContext;
        _speciesRepository = speciesRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteBreedCommand command, 
        CancellationToken cancellationToken = default)
    {
        var petResult = await _readDbContext.Pets
            .FirstOrDefaultAsync(p => p.BreedId == command.BreedId, cancellationToken);
        
        if (petResult != null)
            return Errors.Breed.IsCurrentlyUsed().ToErrorList();
        
        await _speciesRepository.DeleteBreed(
            BreedId.Create(command.BreedId), 
            cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return Guid.NewGuid();
    }
}