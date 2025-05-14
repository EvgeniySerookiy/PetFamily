using System.Text.Json;
using CSharpFunctionalExtensions;
using Dapper;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Models;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.PetManagement.Queries.Volunteers.GetPet;

public class GetPetHandler : IQueryHandlerPet<PagedList<PetDto>, GetPetQuery>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetPetHandler(
        ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    public async Task<Result<PetDto, ErrorList>> Handle(
        GetPetQuery query, CancellationToken cancellationToken = default)
    {
        var connection = _sqlConnectionFactory.Create();

        var parameters = new DynamicParameters();
        parameters.Add("@VolunteerId", query.VolunteerId);
        parameters.Add("@PetId", query.PetId);
        
        var sql = $"""
                   SELECT id, volunteer_id, pet_name, date_of_birth,
                          species_id, breed_id, color, region, city,
                          position, pet_photos
                   FROM pets
                   WHERE id = @PetId 
                   AND volunteer_id = @VolunteerId
                   AND is_deleted = FALSE
                   LIMIT 1
                   """;
        
        var pets = await connection.QueryAsync<PetDto, string, PetDto>(
            sql, (pet, jsonPhotos) =>
            {
                var photos = JsonSerializer.Deserialize<PetPhotoDto[]>(jsonPhotos) ?? [];
                pet.PetPhotos = photos;
                return pet;
            },
            splitOn: "pet_photos",
            param: parameters);

        var pet = pets.FirstOrDefault();
        if (pet == null)
        {
            return Errors.Pet.NotFound(query.PetId).ToErrorList();
        }
            
        
        return new PetDto
        {
            Id = pet.Id,
            VolunteerId = pet.VolunteerId,
            SpeciesId = pet.SpeciesId,
            BreedId = pet.BreedId,
            PetName = pet.PetName,
            Color = pet.Color,
            Region = pet.Region,
            City = pet.City,
            Position = pet.Position,
            PetPhotos = pet.PetPhotos
        };
    }
}