using Common.EventBus.NotificationMessage;
using Streamone.Integrations.BuildingBlocks.EventBus.Events;
using System;
using System.Text.Json.Serialization;

namespace Common.EventBus
{
    public record MessageSentIntegrationEvent : IntegrationEvent
    {
        public Guid MessageId { get; }

        [JsonPropertyName("header")]
        public MessageHeader Header { get; }
        [JsonPropertyName("body")]
        public string MessageBody { get; init; }

        [JsonConstructor]
        public MessageSentIntegrationEvent(MessageHeader header, string messageBody)
        {
            MessageId = Guid.NewGuid(); // Generate a unique ID for the message
            Header = header;
            MessageBody = messageBody;
        }
    }

    public record MessageHeader
    {
        public string Subject { get; }
        public Destination Recipients { get; }
        public string Source { get; }
        public MessageAlertLevel AlertLevel { get; }
        public string SourceDefinedId { get; }
        public string FromAddress { get; }

        [JsonConstructor]
        public MessageHeader(
            string subject,
            Destination recipients,
            string source,
            MessageAlertLevel alertLevel,
            string sourceDefinedId,
            string fromAddress)
        {
            Subject = subject;
            Recipients = recipients;
            Source = source;
            AlertLevel = alertLevel;
            SourceDefinedId = sourceDefinedId;
            FromAddress = fromAddress;
        }
    }

}
