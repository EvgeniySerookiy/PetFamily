using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.Update;

namespace PetFamily.Application;

public static class Inject
{
    public static IServiceCollection AddAplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();
        services.AddScoped<UpdateMainInfoHandler>();
        services.AddScoped<UpdateCollectionRequisitesForHelpHandler>();
        services.AddScoped<UpdateCollectionSocialNetworkHandler>();
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        return services;
    }
}