using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Response;
using PetFamily.Application.Volunteers.CreateVolunteer;
using PetFamily.Application.Volunteers.DTOs;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromServices] CreateVolunteerHandler handler,
        [FromServices] IValidator<CreateVolunteerDto> validator,
        [FromBody] CreateVolunteerDto request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ToValidationErrorResponse();

        var result = await handler.Handle(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return CreatedAtAction("", result.Value);
    }
}