using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos;
using PetFamily.Application.Models;
using PetFamily.Application.PetManagement.Commands.Pets.AddPet;
using PetFamily.Application.PetManagement.Commands.Pets.AddPetPhotos;
using PetFamily.Application.PetManagement.Commands.Pets.MovePets;
using PetFamily.Application.PetManagement.Commands.Volunteers.Create;
using PetFamily.Application.PetManagement.Commands.Volunteers.Delete;
using PetFamily.Application.PetManagement.Commands.Volunteers.Restore;
using PetFamily.Application.PetManagement.Commands.Volunteers.Update.UpdateMainInfo;
using PetFamily.Application.PetManagement.Commands.Volunteers.Update.UpdateRequisitesForHelp;
using PetFamily.Application.PetManagement.Commands.Volunteers.Update.UpdateSocialNetwork;
using PetFamily.Application.PetManagement.Queries.GetPetsWithPagination;

namespace PetFamily.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddCommands()
            .AddQueries()
            .AddValidatorsFromAssembly(typeof(Inject).Assembly);
        
        return services;
    }

    private static IServiceCollection AddCommands(this IServiceCollection services)
    {
        return services.Scan(scan => scan
            .FromAssemblies(typeof(Inject).Assembly)
            .AddClasses(classes => classes
                .AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
    }
    
    private static IServiceCollection AddQueries(this IServiceCollection services)
    {
        return services.Scan(scan => scan
            .FromAssemblies(typeof(Inject).Assembly)
            .AddClasses(classes => classes
                .AssignableTo(typeof(IQueryHandler<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
    }
}