using Streamone.Integrations.BuildingBlocks.EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.EventBus
{
    public class SBusNotificationTriggerFactory : ISBusNotificationTriggerFactory
    {
        private readonly IEventBus _eventBus;

        public SBusNotificationTriggerFactory(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public SBusNotificationTrigger Create(string sourceName, string sourceDefinedId, string sender)
        {
            return new SBusNotificationTrigger(_eventBus, sourceName, sourceDefinedId, sender);
        }
    }

    public interface ISBusNotificationTriggerFactory
    {
        SBusNotificationTrigger Create(string sourceName, string sourceDefinedId, string sender);
    }
}
