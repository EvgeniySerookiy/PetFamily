using System.Text.Json;
using Dapper;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Models;

namespace PetFamily.Application.PetManagement.Queries.Volunteers.GetPetsWithPagination;

public class GetFilteredPetsWithPaginationHandler : 
    IQueryHandlerPets<PagedList<PetDto>, GetFilteredPetsWithPaginationQuery>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetFilteredPetsWithPaginationHandler(
        ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<PagedList<PetDto>> Handle(
        GetFilteredPetsWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var whereClauses = new List<string> {"1 = 1"};
        var connection = _sqlConnectionFactory.Create();
        var totalCount = await connection.ExecuteScalarAsync<long>(
            @"SELECT COUNT(*) FROM pets");
        
        var parameters = new DynamicParameters();
        parameters.Add("@PageSize", query.PageSize);
        parameters.Add("@Offset", (query.Page - 1) * query.PageSize);
        
        if (query.VolunteerId.HasValue)
        {
            whereClauses.Add("volunteer_id = @VolunteerId");
            parameters.Add("@VolunteerId", query.VolunteerId);
        }
        
        if (!string.IsNullOrWhiteSpace(query.PetName))
        {
            whereClauses.Add("pet_name ILIKE @PetName");
            parameters.Add("@PetName", $"%{query.PetName}%");
        }
        
        if (query.MinAge.HasValue)
        {
            whereClauses.Add("date_part('year', age(NOW(), date_of_birth)) >= @MinAge");
            parameters.Add("@MinAge", query.MinAge); 
        }
        
        if (query.MaxAge.HasValue)
        {
            whereClauses.Add("date_part('year', age(NOW(), date_of_birth)) <= @MaxAge");
            parameters.Add("@MaxAge", query.MaxAge);
        }
        
        if (query.SpeciesId.HasValue)
        {
            whereClauses.Add("species_id = @SpeciesId");
            parameters.Add("@SpeciesId", query.SpeciesId);
        }
        
        if (query.SpeciesId.HasValue)
        {
            whereClauses.Add("breed_id = @BreedId");
            parameters.Add("@BreedId", query.BreedId);
        }
        
        if (!string.IsNullOrWhiteSpace(query.Color))
        {
            whereClauses.Add("color ILIKE @Color");
            parameters.Add("@Color", $"%{query.Color}%");
        }
        
        if (!string.IsNullOrWhiteSpace(query.Region))
        {
            whereClauses.Add("region ILIKE @Region");
            parameters.Add("@Region", $"%{query.Region}%");
        }
        
        if (!string.IsNullOrWhiteSpace(query.City))
        {
            whereClauses.Add("city ILIKE @City");
            parameters.Add("@City", $"%{query.City}%");
        }
        
        if (query.PositionFrom > 0)
        {
            whereClauses.Add("position >= @PositionFrom");
            parameters.Add("@PositionFrom", query.PositionFrom);
        }

        if (query.PositionTo > 0)
        {
            whereClauses.Add("position <= @PositionTo");
            parameters.Add("@PositionTo", query.PositionTo);
        }
        
        var whereClause = string.Join(" AND ", whereClauses);
        
        var sql = $"""
                      SELECT id, volunteer_id, pet_name, date_of_birth,
                             species_id, breed_id, position, 
                             pet_photos, color, region, city
                      FROM pets
                      WHERE {whereClause}
                      ORDER BY {PetOrderByClauseBuilder.BuildOrderBySql(query.SortBy, query.SortDirection)} 
                      LIMIT @PageSize OFFSET @Offset
                  """;
        
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