//2024 (c) TD Synnex - All Rights Reserved.

using Configuration.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace Common.EventBus.NotificationMessage;

public class MessageAlertLevel : Enumeration
{
    [JsonPropertyName("system")]
    public static readonly MessageAlertLevel System = new(1, nameof(System).ToUpperInvariant());
    [JsonPropertyName("log")]
    public static readonly MessageAlertLevel Log = new(2, nameof(Log).ToUpperInvariant());
    [JsonPropertyName("warning")]
    public static readonly MessageAlertLevel Warning = new(3, nameof(Warning).ToUpperInvariant());
    [JsonPropertyName("error")]
    public static readonly MessageAlertLevel Error = new(4, nameof(Error).ToUpperInvariant());
    [JsonPropertyName("alert")]
    public static readonly MessageAlertLevel Alert = new (5, nameof(Alert).ToUpperInvariant());


    public MessageAlertLevel(int id, string name) : base(id, name)
    {
    }
}
