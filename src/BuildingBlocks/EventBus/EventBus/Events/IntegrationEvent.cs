




namespace Streamone.Integrations.BuildingBlocks.EventBus.Events;

public record IntegrationEvent
{        
    public IntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
    }

    [JsonConstructor]
    public IntegrationEvent(Guid id, DateTime createDate)
    {
        Id = id;
        CreationDate = createDate;
    }
    [JsonConstructor]
    public IntegrationEvent(string bearerToken)
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
        IntegrationField = new(bearerToken);
    }

    [JsonInclude]
    public Guid Id { get; private init; }

    [JsonInclude]
    public DateTime CreationDate { get; private init; }

#nullable enable
    [JsonInclude]
    public virtual IntegrationField? IntegrationField { get; set; } = null;
#nullable disable
}

public record IntegrationField
{
    [JsonInclude]
    public string BearerToken { get; set; } = string.Empty;
    [JsonConstructor]
    public IntegrationField(string bearerToken)
    {
        BearerToken = bearerToken ?? string.Empty;
    }
}