using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.Volunteers.Actions.AddPet;
using PetFamily.Application.Volunteers.Actions.Create;
using PetFamily.Application.Volunteers.Actions.Delete;
using PetFamily.Application.Volunteers.Actions.Restore;
using PetFamily.Application.Volunteers.Actions.Update.UpdateMainInfo;
using PetFamily.Application.Volunteers.Actions.Update.UpdateRequisitesForHelp;
using PetFamily.Application.Volunteers.Actions.Update.UpdateSocialNetwork;
using PetFamily.Application.Volunteers.DTOs;
using PetFamily.Application.Volunteers.DTOs.Collections;
using PetFamily.Infrastructure.Models;

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

    [HttpPut("{id:guid}/restore")]
    public async Task<ActionResult> Restore(
        [FromRoute] Guid id,
        [FromServices] RestoreVolunteerHandler handler,
        [FromServices] IValidator<RestoreVolunteerRequest> validator,
        CancellationToken cancellationToken = default)
    {
        var request = new RestoreVolunteerRequest(id);

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

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(
        [FromRoute] Guid id,
        [FromServices] DeleteVolunteerHandler handler,
        [FromServices] IValidator<DeleteVolunteerRequest> validator,
        CancellationToken cancellationToken = default)
    {
        var request = new DeleteVolunteerRequest(id);

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

    [HttpPost("pet-file")]
    public async Task<ActionResult> AddPetFile(
        IFormFile file,
        [FromServices] AddPetHandler handler,
        CancellationToken cancellationToken = default)
    {
        await using var stream = file.OpenReadStream();
        
        var path = Guid.NewGuid().ToString();
        
        var fileData = new FileData(stream, "photos", path);
        
        var result = await handler.Handle(fileData, cancellationToken);
        
        if(result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}/pet-file")]
    public async Task<ActionResult> Delete(
        [FromRoute] Guid id,
        [FromServices] DeletePetHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(id, cancellationToken);
        
        if(result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [HttpPut("{id:guid}/pet-file")]
    public async Task<ActionResult> GetFileUrl(
        [FromRoute] Guid id,
        [FromServices] GetFileDownloadHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(id);
        
        if(result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}