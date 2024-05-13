using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.Base.Domain.DDD.Interfaces;

/// <summary>
/// Used for Domain Driven Design. If you are using Anemic Domain Modeling, use
/// <see cref="IEntityWithGuid"/> for entities with typed identifiers.
/// </summary>
public interface IHasStronglyTypedId
{
    #region [ Public Methods ]
    Guid GetTypedId();

    #endregion
}