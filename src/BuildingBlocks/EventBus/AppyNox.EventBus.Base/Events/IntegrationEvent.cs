using Newtonsoft.Json;

namespace AppyNox.EventBus.Base.Events
{
    public class IntegrationEvent
    {
        #region [ Public Constructors ]

        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.UtcNow;
        }

        [JsonConstructor]
        public IntegrationEvent(Guid id, DateTime createdDate)
        {
            Id = id;
            CreatedDate = createdDate;
        }

        #endregion

        #region [ Properties ]

        [JsonProperty]
        public Guid Id { get; private set; }

        [JsonProperty]
        public DateTime CreatedDate { get; private set; }

        #endregion
    }
}