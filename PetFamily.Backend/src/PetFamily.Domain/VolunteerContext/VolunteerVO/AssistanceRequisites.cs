using PetFamily.Domain.Shared;
using PetFamily.Domain.SharedVO;

namespace PetFamily.Domain.VolunteerContext.VolunteerVO;

public record AssistanceRequisites
{
    public Title Title { get; }
    public Description Description { get; }
    
    private AssistanceRequisites() { }

    private AssistanceRequisites(Title title, Description description)
    {
        Title = title;
        Description = description;
    }
    
    public static Result<AssistanceRequisites> Create(Title title, Description description)
    {
        var requisitesForHelp = new AssistanceRequisites(title, description);
        
        return requisitesForHelp;
    }
}