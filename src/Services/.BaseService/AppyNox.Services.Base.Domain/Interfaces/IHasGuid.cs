namespace AppyNox.Services.Base.Domain.Interfaces;

/// <summary>
/// Used for Domain Driven Design StronglyTypedIds. Use <see cref="IHasStronglyTypedId"/> for aggregates.
/// </summary>
public interface IHasGuidId
{
    #region [ Public Methods ]

    Guid GetGuidValue();

    #endregion
}