namespace AppyNox.Services.Base.Core.AsyncLocals
{
    /// <summary>
    /// Provides a context for storing and retrieving the current request's correlation ID.
    /// </summary>
    public static class NoxContext
    {
        #region [ Fields ]

        private static readonly AsyncLocal<Guid> _correlationId = new();

        private static readonly AsyncLocal<Guid> _userId = new();

        private static readonly AsyncLocal<Guid> _companyId = new();

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the correlation ID for the current request.
        /// </summary>
        public static Guid CorrelationId
        {
            get => _correlationId.Value;
            set => _correlationId.Value = value;
        }

        /// <summary>
        /// Gets or sets the User ID for the current request.
        /// </summary>
        public static Guid UserId
        {
            get => _userId.Value;
            set => _userId.Value = value;
        }

        /// <summary>
        /// Gets or sets the Company ID for the current request.
        /// </summary>
        public static Guid CompanyId
        {
            get => _companyId.Value;
            set => _companyId.Value = value;
        }

        #endregion
    }
}