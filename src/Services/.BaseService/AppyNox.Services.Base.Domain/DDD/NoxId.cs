using AppyNox.Services.Base.Domain.DDD.Interfaces;

namespace AppyNox.Services.Base.Domain.DDD;

/// <summary>
/// Used for Domain Driven Design StronglyTypedIds. Use <see cref="IHasStronglyTypedId"/> for aggregates.
/// </summary>
public abstract record NoxId(Guid Value)
{
    public static implicit operator Guid(NoxId instance)
    {
        return instance.Value;
    }
}