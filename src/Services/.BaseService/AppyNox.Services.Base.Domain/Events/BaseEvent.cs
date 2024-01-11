namespace AppyNox.Services.Base.Domain.Events
{
    public abstract class BaseEvent
    {
        #region [ Properties ]

        public DateTime Timestamp { get; private set; } = DateTime.UtcNow;

        #endregion
    }
}