using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Volunteers.Actions.Pets.AddPet;
using PetFamily.Application.Volunteers.Actions.Pets.AddPetPhotos;
using PetFamily.Application.Volunteers.Actions.Pets.MovePets;
using PetFamily.Application.Volunteers.Actions.Volunteers.Create;
using PetFamily.Application.Volunteers.Actions.Volunteers.Delete;
using PetFamily.Application.Volunteers.Actions.Volunteers.Restore;
using PetFamily.Application.Volunteers.Actions.Volunteers.Update.UpdateMainInfo;
using PetFamily.Application.Volunteers.Actions.Volunteers.Update.UpdateRequisitesForHelp;
using PetFamily.Application.Volunteers.Actions.Volunteers.Update.UpdateSocialNetwork;

namespace PetFamily.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();
        services.AddScoped<UpdateMainInfoHandler>();
        services.AddScoped<UpdateRequisitesForHelpHandler>();
        services.AddScoped<UpdateSocialNetworkHandler>();
        services.AddScoped<DeleteVolunteerHandler>();
        services.AddScoped<RestoreVolunteerHandler>();
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        services.AddScoped<AddPetHandler>();
        services.AddScoped<AddPetPhotosHandler>();
        services.AddScoped<MovePetsHandler>();
        return services;
    }
}