using AppyNox.Services.Base.Domain.DDD.Interfaces;

namespace AppyNox.Services.Base.Domain.Interfaces;

/// <summary>
/// Used for Anemic Domain Modeling. If you are using Domain Driven Design, use
/// <see cref="IHasStronglyTypedId"/> for entities with typed identifiers.
/// </summary>
public interface IEntityWithGuid
{
    #region [ Properties ]

    public Guid Id { get; set; }

    #endregion
}