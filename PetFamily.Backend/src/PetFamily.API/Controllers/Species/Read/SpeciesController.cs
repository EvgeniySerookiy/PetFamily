using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Species.Read.Request;
using PetFamily.Application.PetManagement.Queries.Species.GetBreedsOfSpeciesWithPagination;
using PetFamily.Application.PetManagement.Queries.Species.GetSpeciesWithPagination;
using PetFamily.Domain.SpeciesManagement.SpeciesVO;

namespace PetFamily.API.Controllers.Species.Read;

public class SpeciesController : ApplicationController
{
    [HttpGet("{id:guid}/breeds")]
    public async Task<ActionResult> GetBreeds(
        [FromRoute] Guid id,
        [FromQuery] GetBreedsOfSpeciesWithPaginationRequest request,
        [FromServices] GetBreedsOfSpeciesWithPaginationHandler handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetBreedsOfSpeciesWithPaginationQuery(
            SpeciesId.Create(id).Value,
            request.Page,
            request.PageSize);

        var response = await handler.Handle(query, cancellationToken);

        return Ok(response);
    }

    [HttpGet("species")]
    public async Task<ActionResult> GetSpecies(
        [FromQuery] GetSpeciesWithPaginationRequest request,
        [FromServices] GetSpeciesWithPaginationHandler handler,
        CancellationToken cancellationToken = default)
    {
        var query = request.ToQuery();

        var response = await handler.Handle(query, cancellationToken);

        return Ok(response);
    }
}