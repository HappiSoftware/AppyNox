namespace AppyNox.EventBus.Base
{
    public class SubscriptionInfo
    {
        #region [ Public Constructors ]

        public SubscriptionInfo(Type handlerType)
        {
            HandlerType = handlerType ?? throw new ArgumentNullException(nameof(handlerType));
        }

        #endregion

        #region [ Properties ]

        public Type HandlerType { get; private set; }

        #endregion
    }
}