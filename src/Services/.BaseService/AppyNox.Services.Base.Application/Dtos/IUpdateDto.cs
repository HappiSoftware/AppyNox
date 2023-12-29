namespace AppyNox.Services.Base.Application.Dtos
{
    /// <summary>
    /// Defines the contract for update data transfer objects (DTOs).
    /// </summary>
    public interface IUpdateDto
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the identifier for the entity to be updated.
        /// </summary>
        Guid Id { get; set; }

        #endregion
    }
}