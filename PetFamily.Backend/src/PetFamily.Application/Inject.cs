using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Volunteers.Actions.Create;
using PetFamily.Application.Volunteers.Actions.Delete;
using PetFamily.Application.Volunteers.Actions.Restore;
using PetFamily.Application.Volunteers.Actions.Update.UpdateMainInfo;
using PetFamily.Application.Volunteers.Actions.Update.UpdateRequisitesForHelp;
using PetFamily.Application.Volunteers.Actions.Update.UpdateSocialNetwork;

namespace PetFamily.Application;

public static class Inject
{
    public static IServiceCollection AddAplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();
        services.AddScoped<UpdateMainInfoHandler>();
        services.AddScoped<UpdateCollectionRequisitesForHelpHandler>();
        services.AddScoped<UpdateCollectionSocialNetworkHandler>();
        services.AddScoped<DeleteVolunteerHandler>();
        services.AddScoped<RestoreVolunteerHandler>();
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        return services;
    }
}