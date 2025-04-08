namespace PetFamily.Application.Dtos;

public class PetDto
{
    public Guid Id { get; init; }
    public Guid VolunteerId { get; init; }
    public string PetName { get; init; } = string.Empty;
    public int Position { get; init; }
    public PetPhotoDto[] PetPhotos { get; set; } = null!;
}

public class PetPhotoDto
{
    public string PhotoPath { get; init; } = string.Empty;
    public float Size { get; init; }
}