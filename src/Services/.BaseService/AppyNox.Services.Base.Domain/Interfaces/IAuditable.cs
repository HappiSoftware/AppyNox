namespace AppyNox.Services.Base.Domain.Interfaces;

public interface IAuditable
{
    string CreatedBy { get; }
    DateTime CreationDate { get; }
    string? UpdatedBy { get; }
    DateTime? UpdateDate { get; }
}