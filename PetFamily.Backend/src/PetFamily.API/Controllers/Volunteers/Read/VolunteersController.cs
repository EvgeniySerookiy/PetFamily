using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Volunteers.Read.Requests;
using PetFamily.API.Controllers.Volunteers.Write.Requests;
using PetFamily.Application.PetManagement.Queries.Volunteers.GetPetsWithPagination;
using PetFamily.Application.PetManagement.Queries.Volunteers.GetVolunteer;
using PetFamily.Application.PetManagement.Queries.Volunteers.GetVolunteersWithPagination;

namespace PetFamily.API.Controllers.Volunteers.Read;

public class VolunteersController : ApplicationController
{
    // Операции с волонтером
    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetVolunteer(
        [FromRoute] Guid id,
        [FromServices] GetVolunteerHandler handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetVolunteerQuery(id);
        
        var result = await handler.Handle(query, cancellationToken);
        
        return Ok(result.Value);
    }
    
    [HttpGet]
    public async Task<ActionResult> GetVolunteers(
        [FromQuery] GetFilteredVolunteersWithPaginationRequest request,
        [FromServices] GetFilteredVolunteersWithPaginationHandler handler,
        CancellationToken cancellationToken = default)
    {
        var query = request.ToQuery();
        
        var response = await handler.Handle(query, cancellationToken);
        
        return Ok(response);
    }
    
    // Операции с петом
    [HttpGet("pets/dapper")]
    public async Task<ActionResult> GetPetsDapper(
        [FromQuery] GetFilteredPetsWithPaginationRequest request,
        [FromServices] GetFilteredPetsWithPaginationHandlerPetsDapper handlerPets,
        CancellationToken cancellationToken = default)
    {
        var query = request.ToQuery();
        
        var response = await handlerPets.Handle(query, cancellationToken);
        
        return Ok(response);
    }
    
    [HttpGet("pets")]
    public async Task<ActionResult> GetPets(
        [FromQuery] GetFilteredPetsWithPaginationRequest request,
        [FromServices] GetFilteredPetsWithPaginationHandlerPets handlerPets,
        CancellationToken cancellationToken = default)
    {
        var query = request.ToQuery();
        
        var response = await handlerPets.Handle(query, cancellationToken);
        
        return Ok(response);
    }
}