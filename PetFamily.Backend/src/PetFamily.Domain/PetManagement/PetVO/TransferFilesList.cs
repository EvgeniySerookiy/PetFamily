using CSharpFunctionalExtensions;

namespace PetFamily.Domain.PetManagement.PetVO;

public record TransferFilesList
{
    private readonly List<PetPhoto> _photos = new();
    public IReadOnlyList<PetPhoto> Photos => _photos;
    
    private TransferFilesList() {}
    
    private TransferFilesList(IEnumerable<PetPhoto> photos)
    {
        _photos = photos.ToList();
    }
    
    public void AddPhoto(PetPhoto petPhoto)
    {
        _photos.Add(petPhoto);
    }

    public void DeletePhotos(List<PetPhoto> petPhotos)
    {
        _photos.RemoveAll(petPhotos.Contains);
    }
    
    public static Result<TransferFilesList> Create(IEnumerable<PetPhoto> photos)
    {
        
        return new TransferFilesList(photos);
    }
}