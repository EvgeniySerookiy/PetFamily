namespace PetFamily.Application.PetManagement.Queries.Volunteers.GetPetsWithPagination;

public static class PetOrderByClauseBuilder
{
    public static string BuildOrderBySql(SortBy? sortBy, SortDirection? sortDirection)
    {
        string baseOrderByString = "position";
        string directionString = "ASC";

        if (sortBy.HasValue)
        {
            switch (sortBy.Value)
            {
                case SortBy.VolunteerId: baseOrderByString = "volunteer_id"; break;
                case SortBy.SpeciesId: baseOrderByString = "species_id"; break;
                case SortBy.BreedId: baseOrderByString = "breed_id"; break;
                case SortBy.PetName: baseOrderByString = "pet_name"; break;
                case SortBy.DateOfBirth: baseOrderByString = "date_of_birth"; break;
                case SortBy.Color: baseOrderByString = "color"; break;
                case SortBy.Region: baseOrderByString = "region"; break;
                case SortBy.City: baseOrderByString = "city"; break;

                default:
                    baseOrderByString = "position";
                    break;
            }
        }

        if (sortDirection.HasValue && sortDirection.Value == SortDirection.Desc)
        {
            directionString = "DESC";
        }

        string orderByClause = $"{baseOrderByString} {directionString}";

        return orderByClause;
    }
}