using PetFamily.Application.Volunteers.PetDTOs;

namespace PetFamily.API.Processors;

public class FormFileProcessor : IAsyncDisposable
{
    private readonly List<CreateFileDto> _fileDtos = [];

    public List<CreateFileDto> Process(IFormFileCollection files)
    {
        foreach (var file in files)
        {
            var stream = file.OpenReadStream(); 
            var filesDto = new CreateFileDto(stream, file.FileName, file.ContentType);
            _fileDtos.Add(filesDto);
        }
        
        return _fileDtos;
    }
    
    public async ValueTask DisposeAsync()
    {
        foreach (var file in _fileDtos)
        {
            await file.Content.DisposeAsync();
        }
    }
}