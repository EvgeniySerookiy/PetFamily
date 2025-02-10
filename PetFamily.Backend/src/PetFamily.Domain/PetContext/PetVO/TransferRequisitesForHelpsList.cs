using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetContext.PetVO;

public record TransferRequisitesForHelpsList
{
    private readonly List<RequisitesForHelp> _requisitesForHelps = new();
    public IReadOnlyList<RequisitesForHelp> RequisitesForHelps => _requisitesForHelps;
    
    private TransferRequisitesForHelpsList() {}
    private TransferRequisitesForHelpsList(IEnumerable<RequisitesForHelp> requisitesForHelps)
    {
        _requisitesForHelps = requisitesForHelps.ToList();
    }
    
    public void AddRequisitesForHelp(RequisitesForHelp requisitesForHelp)
    {
        _requisitesForHelps.Add(requisitesForHelp);
    }

    public static Result<TransferRequisitesForHelpsList> Create(IEnumerable<RequisitesForHelp> requisitesForHelps)
    {
        return new TransferRequisitesForHelpsList(requisitesForHelps);
    }
}