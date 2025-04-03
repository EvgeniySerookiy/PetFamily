namespace PetFamily.Application.PetManagement.Queries;

public record GetPetsWithPaginationQuery(
    int Page,
    int PageSize);