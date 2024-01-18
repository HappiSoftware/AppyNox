using AppyNox.Services.Base.Core.ExceptionExtensions;

namespace AppyNox.Services.Base.Core.AsyncLocals
{
    /// <summary>
    ///  Provides a context for storing and retrieving the current request's user ID.
    /// </summary>
    public static class UserIdContext
    {
        #region [ Fields ]

        private static readonly AsyncLocal<Guid> _userId = new();

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the User ID for the current request.
        /// </summary>
        public static Guid UserId
        {
            get => _userId.Value;
            set => _userId.Value = value;
        }

        #endregion
    }
}