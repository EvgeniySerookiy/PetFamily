using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SpeciesManagement.Entities;
using PetFamily.Domain.SpeciesManagement.SpeciesVO;

namespace PetFamily.Application.PetManagement.Commands.SpeciesСmd.AddSpecies;

public class AddSpeciesHandler : ICommandHandler<Guid, AddSpeciesCommand>
{
    private readonly ISpeciesReadRepository _speciesReadRepository;
    private readonly ISpeciesWriteRepository _speciesWriteRepository;
    private readonly IValidator<AddSpeciesCommand> _validator;
    
    public AddSpeciesHandler(
        ISpeciesReadRepository speciesReadRepository,
        ISpeciesWriteRepository speciesWriteRepository,
        IValidator<AddSpeciesCommand> validator)
    {
        _speciesReadRepository = speciesReadRepository;
        _speciesWriteRepository = speciesWriteRepository;
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
        
        var speciesResult = await _speciesReadRepository
            .GetSpeciesIdByName(command.SpeciesName, cancellationToken);
        if (speciesResult.IsSuccess)
            return speciesResult.Error.ToErrorList();
        
        var speciesToCreate = Species.Create(
            speciesId, 
            speciesName, 
            []);
        
        await _speciesWriteRepository.Add(speciesToCreate.Value, cancellationToken);
        
        return speciesToCreate.Value.Id.Value;
    }
}