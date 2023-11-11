namespace AppyNox.Services.Coupon.Application.ExceptionExtensions
{
    public class DetailLevelNotFoundException : Exception
    {
        #region [ Public Constructors ]

        public DetailLevelNotFoundException()
        { }

        public DetailLevelNotFoundException(string message)
            : base(message)
        { }

        public DetailLevelNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }

        #endregion
    }
}