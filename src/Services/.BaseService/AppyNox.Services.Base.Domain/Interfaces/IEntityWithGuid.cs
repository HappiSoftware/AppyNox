namespace AppyNox.Services.Base.Domain.Interfaces
{
    /// <summary>
    /// Defines a contract for entities with a GUID identifier.
    /// </summary>
    public interface IEntityWithGuid
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        Guid Id { get; set; }

        #endregion
    }
}