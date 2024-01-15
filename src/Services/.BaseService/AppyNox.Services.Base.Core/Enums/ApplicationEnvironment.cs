namespace AppyNox.Services.Base.Core.Enums
{
    /// <summary>
    /// Represents the application's running environment.
    /// </summary>
    public enum ApplicationEnvironment
    {
        /// <summary>
        /// The development environment.
        /// </summary>
        Development,

        /// <summary>
        /// The staging environment, typically used for pre-production testing.
        /// </summary>
        Staging,

        /// <summary>
        /// The production environment, used for live operations.
        /// </summary>
        Production,
    }
}