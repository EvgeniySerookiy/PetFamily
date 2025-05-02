using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SpeciesManagement.Entities;
using PetFamily.Domain.SpeciesManagement.SpeciesVO;

namespace PetFamily.Application.PetManagement.Commands.SpeciesСmd.AddSpecies;

public class AddSpeciesHandler : ICommandHandler<Guid, AddSpeciesCommand>
{
    private readonly IReadDbContext _readDbContext;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IValidator<AddSpeciesCommand> _validator;
    
    public AddSpeciesHandler(
        IReadDbContext readDbContext,
        ISpeciesRepository speciesRepository,
        IValidator<AddSpeciesCommand> validator)
    {
        _readDbContext = readDbContext;
        _speciesRepository = speciesRepository;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        AddSpeciesCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var speciesId = SpeciesId.NewSpeciesId();

        var speciesName = SpeciesName.Create(
            command.SpeciesName).Value;
        
        var species = await _readDbContext.Species
            .FirstOrDefaultAsync(s => s.SpeciesName == command.SpeciesName, cancellationToken);
        if (species != null)
            return Errors.Species.AlreadyExist().ToErrorList();
        
        var speciesToCreate = Species.Create(
            speciesId, 
            speciesName, 
            []);
        
        await _speciesRepository.Add(speciesToCreate.Value, cancellationToken);
        
        return speciesToCreate.Value.Id.Value;
    }
}