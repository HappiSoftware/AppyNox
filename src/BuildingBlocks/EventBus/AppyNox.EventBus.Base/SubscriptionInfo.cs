namespace AppyNox.EventBus.Base
{
    public class SubscriptionInfo
    {
        public Type HandlerType { get; private set; }
        
        public SubscriptionInfo(Type handlerType)
        {
            HandlerType = handlerType ?? throw new ArgumentNullException(nameof(handlerType));
        }
    }
}
