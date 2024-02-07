namespace AppyNox.Services.Base.Domain.Interfaces;

public interface IEntityTypeId
{
    #region [ Public Methods ]

    Guid GetTypedId { get; }

    #endregion
}