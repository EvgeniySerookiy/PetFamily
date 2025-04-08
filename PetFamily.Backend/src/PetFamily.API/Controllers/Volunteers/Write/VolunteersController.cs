using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Volunteers.Write.Requests;
using PetFamily.API.Extensions;
using PetFamily.API.Processors;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos.VolunteerDTOs;
using PetFamily.Application.PetManagement.Commands.Pets.AddPet;
using PetFamily.Application.PetManagement.Commands.Pets.AddPetPhotos;
using PetFamily.Application.PetManagement.Commands.Pets.DeletePetPhotos;
using PetFamily.Application.PetManagement.Commands.Pets.MovePets;
using PetFamily.Application.PetManagement.Commands.Volunteers.Create;
using PetFamily.Application.PetManagement.Commands.Volunteers.Delete;
using PetFamily.Application.PetManagement.Commands.Volunteers.Restore;
using PetFamily.Application.PetManagement.Commands.Volunteers.Update.UpdateMainInfo;
using PetFamily.Application.PetManagement.Commands.Volunteers.Update.UpdateRequisitesForHelp;
using PetFamily.Application.PetManagement.Commands.Volunteers.Update.UpdateSocialNetwork;

namespace PetFamily.API.Controllers.Volunteers.Write;

public class VolunteersController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromBody] CreateVolunteerRequest request,
        [FromServices] CreateVolunteerHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/main-info")]
    public async Task<ActionResult> Create(
        [FromRoute] Guid id,
        [FromBody] MainInfoDto request,
        [FromServices] UpdateMainInfoHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateMainInfoCommand(id, request);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }


    [HttpPut("{id:guid}/requisites-for-help")]
    public async Task<ActionResult> Create(
        [FromRoute] Guid id,
        [FromBody] UpdateRequisitesForHelpRequest request,
        [FromServices] UpdateRequisitesForHelpHandler handler,
        CancellationToken cancellationToken = default)
    {
        var updateRequisitesForHelpCommand = new UpdateRequisitesForHelpCommand(id, request.RequisitesForHelps);
        
        var result = await handler.Handle(updateRequisitesForHelpCommand, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/social-network")]
    public async Task<ActionResult> Create(
        [FromRoute] Guid id,
        [FromBody] UpdateSocialNetworkRequest request,
        [FromServices] UpdateSocialNetworkHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateSocialNetworksCommand(id, request.SocialNetworks);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/restore")]
    public async Task<ActionResult> Restore(
        [FromRoute] Guid id,
        [FromServices] RestoreVolunteerHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(new RestoreVolunteerCommand(id), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(
        [FromRoute] Guid id,
        [FromServices] DeleteVolunteerHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(new DeleteVolunteerCommand(id), cancellationToken);
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
        var result = await handler.Handle(request.ToCommand(id), cancellationToken);
        if(result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }

    [HttpPost("{id:guid}/pet")]
    public async Task<ActionResult> AddPet(
        [FromRoute] Guid id,
        [FromForm] MainPetInfoRequest request,
        [FromServices] ICommandHandler<Guid, MainPetInfoCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(id), cancellationToken);
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
        
        var command = new AddPetPhotosCommand(volunteerId, petId, fileDtos);
        
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