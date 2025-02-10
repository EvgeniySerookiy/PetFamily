using PetFamily.Domain.Shared;
using PetFamily.Domain.SharedVO;

namespace PetFamily.Domain.PetContext.PetVO;

public record RequisitesForHelp
{
    public Title Title { get; }
    public Description Description { get; }
    
    private RequisitesForHelp() { }

    private RequisitesForHelp(Title title, Description description)
    {
        Title = title;
        Description = description;
    }

    public static Result<RequisitesForHelp> Create(Title title, Description description)
    {
        var requisitesForHelp = new RequisitesForHelp(title, description);
        
        return requisitesForHelp;
    }
}