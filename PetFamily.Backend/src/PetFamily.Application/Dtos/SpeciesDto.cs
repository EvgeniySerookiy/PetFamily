namespace PetFamily.Application.Dtos;

public class SpeciesDto
{
    public Guid Id { get; init; }
    public string SpeciesName { get; init; }
    public List<BreedDto> Breeds{ get; init; }
}