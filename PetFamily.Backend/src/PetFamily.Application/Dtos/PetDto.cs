namespace PetFamily.Application.Dtos;

public class PetDto
{
    public Guid Id { get; init; }
    public Guid VolunteerId { get; init; }
    public Guid SpeciesId { get; init; }
    public Guid BreedId { get; init; }
    public string PetName { get; init; }
    public DateTime DateOfBird { get; init; }
    public string Color { get; init; }
    public string Region { get; init; }
    public string City { get; init; }
    public int Position { get; init; }
    public bool IsDeleted { get; init; }
    public PetPhotoDto[] PetPhotos { get; set; }
}

public class PetPhotoDto
{
    public string PhotoPath { get; init; }
    public float Size { get; init; }
}