using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Pets.Requests;
using PetFamily.Application.PetManagement.Queries.GetPetsWithPagination;

namespace PetFamily.API.Controllers.Pets;

public class PetsController : ApplicationController
{
    [HttpGet("dapper")]
    public async Task<ActionResult> GetDapper(
        [FromQuery] GetFilteredPetsWithPaginationRequest request,
        [FromServices] GetFilteredPetsWithPaginationHandlerDapper handler,
        CancellationToken cancellationToken = default)
    {
        var query = request.ToQuery();
        
        var response = await handler.Handle(query, cancellationToken);
        
        return Ok(response);
    }
    
    [HttpGet]
    public async Task<ActionResult> Get(
        [FromQuery] GetFilteredPetsWithPaginationRequest request,
        [FromServices] GetFilteredPetsWithPaginationHandler handler,
        CancellationToken cancellationToken = default)
    {
        var query = request.ToQuery();
        
        var response = await handler.Handle(query, cancellationToken);
        
        return Ok(response);
    }
}