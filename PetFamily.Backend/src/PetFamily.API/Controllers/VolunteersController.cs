using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Contracts;
using PetFamily.API.Extensions;
using PetFamily.API.Processors;
using PetFamily.Application.Volunteers.Actions.Pets.AddPet;
using PetFamily.Application.Volunteers.Actions.Pets.AddPetPhotos;
using PetFamily.Application.Volunteers.Actions.Pets.DeletePetPhotos;
using PetFamily.Application.Volunteers.Actions.Pets.MovePets;
using PetFamily.Application.Volunteers.Actions.Volunteers.Create;
using PetFamily.Application.Volunteers.Actions.Volunteers.Delete;
using PetFamily.Application.Volunteers.Actions.Volunteers.Restore;
using PetFamily.Application.Volunteers.Actions.Volunteers.Update.UpdateMainInfo;
using PetFamily.Application.Volunteers.Actions.Volunteers.Update.UpdateRequisitesForHelp;
using PetFamily.Application.Volunteers.Actions.Volunteers.Update.UpdateSocialNetwork;
using PetFamily.Application.Volunteers.PetDTOs;
using PetFamily.Application.Volunteers.PetDTOs.Collections;
using PetFamily.Application.Volunteers.VolunteerDTOs;
using PetFamily.Application.Volunteers.VolunteerDTOs.Collections;

namespace PetFamily.API.Controllers;

public class VolunteersController : ApplicationController
{
    private const string BUCKET_NAME = "photos";
    
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
    
    [HttpPut("{id:guid}/move-pets")]
    public async Task<ActionResult> MovePets(
        [FromRoute] Guid id,
        [FromForm] MovePetsRequest request,
        [FromServices] MovePetsHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new MovePetsCommand(id, request.CurrentPosition, request.ToPosition);
        
        var result = await handler.Handle(command, cancellationToken);
        if(result.IsFailure)
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

    [HttpPost("{id:guid}/pet")]
    public async Task<ActionResult> AddPet(
        [FromRoute] Guid id,
        [FromForm] MainPetInfoDto request,
        [FromServices] AddPetHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new AddPetCommand(id, request);
        
        var result = await handler.Handle(command, cancellationToken);
        if(result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [HttpPost("{volunteerId:guid}/pets/{petId:guid}/photos")]
    public async Task<ActionResult> AddPetPhotos(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromForm] AddPetPhotosRequest request,
        [FromServices] AddPetPhotosHandler handler,
        CancellationToken cancellationToken)
    {
    
        await using var fileProcessor = new FormFileProcessor();
        var fileDtos = fileProcessor.Process(request.Files);
        
        var command = new AddPetPhotosCommand(
            volunteerId,
            petId,
            new CollectionFilesDto(fileDtos));
        
        var result = await handler.Handle(command, cancellationToken);
        if(result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }

    [HttpDelete("{volunteerId:guid}/pets/{petId:guid}/photos")]
    public async Task<ActionResult> Delete(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] DeletePetPhotosRequest request,
        [FromServices] DeletePetPhotosHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeletePetPhotosCommand(volunteerId, petId, request.PhotosId);
        
        var result = await handler.Handle(command, cancellationToken);
        if(result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}