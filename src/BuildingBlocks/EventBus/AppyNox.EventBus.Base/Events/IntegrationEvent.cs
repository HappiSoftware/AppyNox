using Newtonsoft.Json;

namespace AppyNox.EventBus.Base.Events
{
    public class IntegrationEvent
    {
        #region [ Public Constructors ]

        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CorrelationId = "NOT-SET";
            CreatedDate = DateTime.UtcNow;
        }

        [JsonConstructor]
        public IntegrationEvent(Guid id, string correlationId, DateTime createdDate)
        {
            Id = id;
            CorrelationId = correlationId;
            CreatedDate = createdDate;
        }

        #endregion

        #region [ Properties ]

        [JsonProperty]
        public Guid Id { get; private set; }

        [JsonProperty]
        public string CorrelationId { get; set; }

        [JsonProperty]
        public DateTime CreatedDate { get; private set; }

        #endregion
    }
}