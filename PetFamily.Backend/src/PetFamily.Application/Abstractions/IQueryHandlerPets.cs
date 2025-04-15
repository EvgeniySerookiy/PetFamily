using CSharpFunctionalExtensions;
using PetFamily.Application.Dtos;
using PetFamily.Application.Models;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Abstractions;

public interface IQueryHandlerPets<TResponse, in TQuery> where TQuery : IQuery
{
    Task<PagedList<PetDto>> Handle(
        TQuery query, 
        CancellationToken cancellationToken = default);
}

public interface IQueryHandlerSpecies<TResponse, in TQuery> where TQuery : IQuery
{
    Task<PagedList<SpeciesDto>> Handle(
        TQuery query, 
        CancellationToken cancellationToken = default);
}

public interface IQueryHandlerBreedsOfSpecies<TResponse, in TQuery> where TQuery : IQuery
{
    Task<PagedList<BreedDto>> Handle(
        TQuery query, 
        CancellationToken cancellationToken = default);
}

public interface IQueryHandlerVolunteer<TResponse, in TQuery> where TQuery : IQuery
{
    Task<Result<VolunteerDto, ErrorList>> Handle(
        TQuery query, 
        CancellationToken cancellationToken = default);
}

public interface IQueryHandlerVolunteers<TResponse, in TQuery> where TQuery : IQuery
{
    Task<PagedList<VolunteerDto>> Handle(
        TQuery query, 
        CancellationToken cancellationToken = default);
}

