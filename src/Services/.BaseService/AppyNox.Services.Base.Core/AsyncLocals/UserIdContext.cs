using AppyNox.Services.Base.Core.ExceptionExtensions;

namespace AppyNox.Services.Base.Core.AsyncLocals
{
    /// <summary>
    ///  Provides a context for storing and retrieving the current request's user ID.
    /// </summary>
    public static class UserIdContext
    {
        #region [ Fields ]

        private static readonly AsyncLocal<string> _userId = new();

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the User ID for the current request.
        /// </summary>
        public static string UserId
        {
            get => _userId.Value ?? throw new AsyncLocalException("UserId is not set!!");
            set => _userId.Value = value;
        }

        #endregion
    }
}