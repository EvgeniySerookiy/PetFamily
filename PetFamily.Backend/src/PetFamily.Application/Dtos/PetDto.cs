namespace PetFamily.Application.Dtos;

public class PetDto
{
    public Guid Id { get; init; }
    public Guid VolunteerId { get; init; }
    public string PetName { get; init; }
    public int Position { get; init; }
    public PetPhotoDto[] PetPhotos { get; set; }
}

public class PetPhotoDto
{
    public string PhotoPath { get; init; }
    public float Size { get; init; }
}