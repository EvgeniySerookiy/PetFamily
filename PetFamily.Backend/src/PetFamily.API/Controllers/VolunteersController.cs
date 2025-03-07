using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.DTOs;
using PetFamily.Application.Volunteers.DTOs.Collections;
using PetFamily.Application.Volunteers.Requests;
using PetFamily.Application.Volunteers.Update;

namespace PetFamily.API.Controllers;

public class VolunteersController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromBody] CreateVolunteerRequest request,
        [FromServices] CreateVolunteerHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/main-info")]
    public async Task<ActionResult> Create(
        [FromRoute] Guid id,
        [FromServices] UpdateMainInfoHandler handler,
        [FromBody] MainInfoDto dto,
        [FromServices] IValidator<UpdateMainInfoRequest> validator,
        CancellationToken cancellationToken = default)
    {
        var request = new UpdateMainInfoRequest(id, dto);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToValidationErrorResponse();
        }

        var result = await handler.Handle(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    
    [HttpPut("{id:guid}/requisites-for-help")]
    public async Task<ActionResult> Create(
        [FromRoute] Guid id,
        [FromServices] UpdateCollectionRequisitesForHelpHandler handler,
        [FromBody] CollectionRequisitesForHelpDto dto,
        [FromServices] IValidator<UpdateCollectionRequisitesForHelpRequest> validator,
        CancellationToken cancellationToken = default)
    {
        var request = new UpdateCollectionRequisitesForHelpRequest(id, dto);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToValidationErrorResponse();
        }

        var result = await handler.Handle(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpPut("{id:guid}/social-network")]
    public async Task<ActionResult> Create(
        [FromRoute] Guid id,
        [FromServices] UpdateCollectionSocialNetworkHandler handler,
        [FromBody] CollectionSocialNetworkDto dto,
        [FromServices] IValidator<UpdateCollectionSocialNetworkRequest> validator,
        CancellationToken cancellationToken = default)
    {
        var request = new UpdateCollectionSocialNetworkRequest(id, dto);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToValidationErrorResponse();
        }

        var result = await handler.Handle(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}