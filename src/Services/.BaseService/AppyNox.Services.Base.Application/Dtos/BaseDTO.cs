namespace AppyNox.Services.Base.Application.Dtos
{
    /// <summary>
    /// Represents the base class for data transfer objects (DTOs) in the application.
    /// </summary>
    public abstract class BaseDto
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets a code associated with the DTO.
        /// </summary>
        public string? Code { get; set; }

        #endregion
    }
}