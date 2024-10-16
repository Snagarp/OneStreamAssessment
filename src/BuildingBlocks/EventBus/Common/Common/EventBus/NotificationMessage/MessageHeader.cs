
using System.Text.Json.Serialization;

namespace Common.EventBus.NotificationMessage
{
    public class MessageHeader
    {
        [JsonPropertyName("subject")]
        public string subject { get; private set; }
        [JsonPropertyName("destination")]
        public Destination? recipients { get; private set; }
        [JsonPropertyName("sourceId")]
        public string source { get; private set; }
        [JsonPropertyName("alertLevel")]
        public MessageAlertLevel alertLevel { get; private set; }
        [JsonPropertyName("sourceDefinedId")]
        public string sourceDefinedId { get; private set; }
        [JsonPropertyName("from")]
        public string fromAddress { get; private set; }

        public MessageHeader(string subject, Destination recipients, string source, MessageAlertLevel alertLevel, string sourceDefinedId, string fromAddress)
        {
            this.subject = subject ?? string.Empty;
            this.recipients = recipients ?? null;
            this.source = source ?? string.Empty;
            this.alertLevel = alertLevel ?? MessageAlertLevel.Log;
            this.sourceDefinedId = sourceDefinedId ?? string.Empty;
            this.fromAddress = fromAddress ?? string.Empty;
        }
    }
}
