namespace PetFamily.Domain.Shared;

public abstract class SoftDeletableEntity
{
    private bool _isDeleted;
    private DateTime _deletionDate;

    public virtual void Delete()
    {
        
    }
}