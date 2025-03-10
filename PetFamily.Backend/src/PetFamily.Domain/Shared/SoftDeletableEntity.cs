namespace PetFamily.Domain.Shared;

public abstract class SoftDeletableEntity<TId> : Entity<TId> where TId : notnull
{
    public bool IsDeleted { get; private set; }
    public DateTime DeletionDate { get; private set; }

    protected SoftDeletableEntity(TId id) : base(id)
    {
    }
    
    public virtual void Delete()
    {
        DeletionDate = DateTime.UtcNow;
        IsDeleted = true;
    }
    
    public virtual void Restore()
    {
        DeletionDate = DateTime.MinValue;
        IsDeleted = false;
    }
}
