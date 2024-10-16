




namespace Streamone.Integrations.BuildingBlocks.EventBus.Abstractions;

public interface IDynamicIntegrationEventHandler
{
    Task Handle(dynamic eventData);
}
