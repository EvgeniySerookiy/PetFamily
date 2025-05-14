using PetFamily.Application.PetManagement.Queries.Volunteers.GetPetsWithPagination;

namespace PetFamily.API.Controllers.Volunteers.Write.Requests;

public record GetFilteredPetsWithPaginationRequest(
    SortBy SortBy,
    SortDirection SortDirection,
    Guid? VolunteerId,
    string? PetName,
    int? MinAge,
    int? MaxAge,
    Guid? SpeciesId,
    Guid? BreedId,
    string? Color,
    string? Region,
    string? City,
    int? PositionFrom,
    int? PositionTo,
    int Page,
    int PageSize)
{
    public GetFilteredPetsWithPaginationQuery ToQuery() =>
        new(SortBy,
            SortDirection,
            VolunteerId, 
            PetName, 
            MinAge, 
            MaxAge,
            SpeciesId,
            BreedId,
            Color,
            Region,
            City,
            PositionFrom, 
            PositionTo, 
            Page, 
            PageSize);
}