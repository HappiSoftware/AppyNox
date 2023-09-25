namespace AppyNox.Services.Authentication.Application.Utilities
{
    public class DetailLevelNotFoundException : Exception
    {
        public DetailLevelNotFoundException()
        { }

        public DetailLevelNotFoundException(string message)
            : base(message)
        { }

        public DetailLevelNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
