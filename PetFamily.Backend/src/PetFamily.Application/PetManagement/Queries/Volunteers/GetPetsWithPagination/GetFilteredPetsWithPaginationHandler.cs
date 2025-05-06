using System.Text.Json;
using Dapper;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.PetManagement.Queries.Volunteers.GetPetsWithPagination;

public class
    GetFilteredPetsWithPaginationHandler : IQueryHandlerPets<PagedList<PetDto>, GetFilteredPetsWithPaginationQuery>
{
    private readonly IReadDbContext _context;

    public GetFilteredPetsWithPaginationHandler(IReadDbContext context)
    {
        _context = context;
    }

    public async Task<PagedList<PetDto>> Handle(
        GetFilteredPetsWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var petsQuery = _context.Pets;

        petsQuery = petsQuery.WhereIf(!string.IsNullOrWhiteSpace(query.PetName),
            p => p.PetName.Contains(query.PetName!));

        petsQuery = petsQuery.WhereIf(query.PositionTo != null,
            p => p.Position <= query.PositionTo!.Value);

        petsQuery = petsQuery.WhereIf(query.PositionFrom != null,
            p => p.Position >= query.PositionFrom!.Value);

        return await petsQuery
            .OrderBy(p => p.Position)
            .ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}

public class GetFilteredPetsWithPaginationHandlerPetsDapper : IQueryHandlerPets<PagedList<PetDto>,
    GetFilteredPetsWithPaginationQuery>

{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetFilteredPetsWithPaginationHandlerPetsDapper(
        ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<PagedList<PetDto>> Handle(
        GetFilteredPetsWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.Create();
        var totalCount = await connection.ExecuteScalarAsync<long>(
            @"SELECT COUNT(*) FROM pets");

        var parameters = new DynamicParameters();
        parameters.Add("@PageSize", query.PageSize);
        parameters.Add("@Offset", (query.Page - 1) * query.PageSize);
        // parameters.Add("@MaxAge", query.MaxAge);
        // parameters.Add("@MinAge", query.MinAge);
        parameters.Add("@VolunteerId", query.VolunteerId);
        // parameters.Add("@SpeciesId", query.SpeciesId);
        // parameters.Add("@BreedId", query.BreedId);
        //parameters.Add("@PetName", $"%{query.PetName}%");
        parameters.Add("@Color", $"%{query.Color}%");
        // parameters.Add("@Region", $"%{query.Region}%");
        // parameters.Add("@City", $"%{query.City}%");

        var sql = """
                      SELECT id, volunteer_id, species_id, breed_id,
                         pet_name, date_of_birth, color, region, city,
                         position, pet_photos 
                  FROM pets
                  
                  WHERE color ILIKE @Color
                  AND volunteer_id = @VolunteerId
                  ORDER BY position 
                  LIMIT @PageSize OFFSET @Offset
                  """;
        
        // WHERE pet_name ILIKE @PetName
        // AND region ILIKE @Region
        // AND city ILIKE @City
        
        // AND species_id = @SpeciesId
        // AND breed_id = @BreedId

        var pets = await connection.QueryAsync<PetDto, string, PetDto>(
            sql.ToString(), (pet, jsonPhotos) =>
            {
                var photos = JsonSerializer.Deserialize<PetPhotoDto[]>(jsonPhotos) ?? [];
                pet.PetPhotos = photos;
                return pet;
            },
            splitOn: "pet_photos",
            param: parameters);

        return new PagedList<PetDto>
        {
            Items = pets.ToList(),
            TotalCount = totalCount,
            PageSize = query.PageSize,
            Page = query.Page
        };
    }
}