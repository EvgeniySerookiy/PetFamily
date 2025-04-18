using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Species.Write.Request;
using PetFamily.API.Extensions;
using PetFamily.Application.PetManagement.Commands.SpeciesСmd.AddBreed;
using PetFamily.Application.PetManagement.Commands.SpeciesСmd.AddSpecies;
using PetFamily.Application.PetManagement.Commands.SpeciesСmd.DeleteBreed;
using PetFamily.Application.PetManagement.Commands.SpeciesСmd.DeleteSpecies;

namespace PetFamily.API.Controllers.Species.Write;

public class SpeciesController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromForm] AddSpeciesRequest request,
        [FromServices] AddSpeciesHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new AddSpeciesCommand(request.SpeciesName);
        
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpGet]
    public async Task<ActionResult> Get(
        [FromQuery] AddSpeciesRequest request,
        [FromServices] AddSpeciesHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new AddSpeciesCommand(request.SpeciesName);
        
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpDelete("{id:guid}/species")]
    public async Task<ActionResult> Delete(
        [FromRoute] Guid id,
        [FromServices] DeleteSpeciesHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteSpeciesCommand(id);
        
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpPost("{id:guid}/breed")]
    public async Task<ActionResult> Create(
        [FromRoute] Guid id,
        [FromForm] AddBreedRequest request,
        [FromServices] AddBreedHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new AddBreedCommand(id, request.Name);
        
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpDelete("{speciesId:guid}/species/{breedId:guid}/breed")]
    public async Task<ActionResult> Delete(
        [FromRoute] Guid speciesId,
        [FromRoute] Guid breedId,
        [FromServices] DeleteBreedHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteBreedCommand(speciesId, breedId);
        
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}