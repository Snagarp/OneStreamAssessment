
namespace Common.EventBus
{
    internal interface ISBusNotificationTrigger
    {
        void TriggerMessageAsync(string subject, string messageBody);

    }
}
