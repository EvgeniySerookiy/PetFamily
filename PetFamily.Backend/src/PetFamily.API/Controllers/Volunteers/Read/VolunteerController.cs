using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Volunteers.Read.Requests;
using PetFamily.Application.PetManagement.Queries.GetVolunteer;
using PetFamily.Application.PetManagement.Queries.GetVolunteersWithPagination;

namespace PetFamily.API.Controllers.Volunteers.Read;

public class VolunteerController : ApplicationController
{
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
}