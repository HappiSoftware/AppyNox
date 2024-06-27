namespace AppyNox.Services.Sso.Application.AsyncLocals
{
    /// <summary>
    ///  Provides a context for storing and retrieving the current request's Company ID.
    /// </summary>
    public static class SsoContext
    {
        #region [ Fields ]

        private static readonly AsyncLocal<Guid> _companyId = new();

        private static readonly AsyncLocal<bool> _isAdmin = new();

        private static readonly AsyncLocal<bool> _isSuperAdmin = new();

        private static readonly AsyncLocal<bool> _isConnectRequest = new();

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the Company ID for the current request.
        /// </summary>
        public static Guid CompanyId
        {
            get => _companyId.Value;
            set => _companyId.Value = value;
        }

        /// <summary>
        /// Gets or sets the IsAdmin for the current request.
        /// </summary>
        public static bool IsAdmin
        {
            get => _isAdmin.Value;
            set => _isAdmin.Value = value;
        }

        /// <summary>
        /// Gets or sets the IsSuperAdmin for the current request.
        /// </summary>
        public static bool IsSuperAdmin
        {
            get => _isSuperAdmin.Value;
            set => _isSuperAdmin.Value = value;
        }

        /// <summary>
        /// Gets or sets the IsConnectRequest for the current request.
        /// </summary>
        public static bool IsConnectRequest
        {
            get => _isConnectRequest.Value;
            set => _isConnectRequest.Value = value;
        }

        #endregion
    }
}