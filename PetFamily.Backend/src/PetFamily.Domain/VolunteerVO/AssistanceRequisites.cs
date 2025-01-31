using CSharpFunctionalExtensions;
using PetFamily.Domain.SharedVO;

namespace PetFamily.Domain.VolunteerVO;

public record AssistanceRequisites
{
    public Title Title { get; }
    public Description Description { get; }

    private AssistanceRequisites(Title title, Description description)
    {
        Title = title;
        Description = description;
    }
    
    public static Result<AssistanceRequisites> Create(Title title, Description description)
    {
        var requisitesForHelp = new AssistanceRequisites(title, description);
        
        return Result.Success(requisitesForHelp);
    }
}