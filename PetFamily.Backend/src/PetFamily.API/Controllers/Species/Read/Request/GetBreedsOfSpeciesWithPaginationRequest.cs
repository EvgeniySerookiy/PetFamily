namespace PetFamily.API.Controllers.Species.Read.Request;

public record GetBreedsOfSpeciesWithPaginationRequest(
    int Page,
    int PageSize);