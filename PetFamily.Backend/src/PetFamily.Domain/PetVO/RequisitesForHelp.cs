using CSharpFunctionalExtensions;
using PetFamily.Domain.SharedVO;

namespace PetFamily.Domain.PetVO;

public record RequisitesForHelp
{
    public Title Title { get; }
    public Description Description { get; }

    private RequisitesForHelp(Title title, Description description)
    {
        Title = title;
        Description = description;
    }

    public static Result<RequisitesForHelp> Create(Title title, Description description)
    {
        var requisitesForHelp = new RequisitesForHelp(title, description);
        
        return Result.Success(requisitesForHelp);
    }
}