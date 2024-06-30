namespace AppyNox.Services.Base.Domain.Interfaces;

public interface ISoftDeletable
{
    bool IsDeleted { get; }
    DateTime? DeletedDate { get; }
    public string? DeletedBy { get; }
    public void MarkAsDeleted();
}
