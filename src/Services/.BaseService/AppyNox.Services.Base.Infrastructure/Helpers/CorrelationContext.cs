namespace AppyNox.Services.Base.Infrastructure.Helpers
{
    /// <summary>
    /// Provides a context for storing and retrieving the current request's correlation ID.
    /// </summary>
    public static class CorrelationContext
    {
        #region [ Fields ]

        private static readonly AsyncLocal<string> _correlationId = new();

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the correlation ID for the current request.
        /// </summary>
        public static string CorrelationId
        {
            get => _correlationId.Value;
            set => _correlationId.Value = value;
        }

        #endregion
    }
}