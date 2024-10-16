using Streamone.Integrations.BuildingBlocks.EventBus.Abstractions;
using Common.EventBus.NotificationMessage;

namespace Common.EventBus
{
    public class SBusNotificationTrigger : ISBusNotificationTrigger
    {
        private readonly IEventBus _eventBus;
 
        private string _sourceName;
        private string _sourceDefinedId; //This is the uid that matters to the sender (i.e. SalesOrderNumber for EBC)
        private string _sender;
        private MessageAlertLevel _alertLevel;

        private Destination _destination = new Destination();


        internal SBusNotificationTrigger(
            IEventBus serviceBusClient, 
            string sourceName, 
            string sourceDefinedId, 
            string sender
        ) {
            _eventBus = serviceBusClient;
            _sourceName = sourceName;
            _sourceDefinedId = sourceDefinedId;
            _sender = sender;

            // default alert level
            _alertLevel = MessageAlertLevel.Log;

            _destination = new Destination();
        }

        public bool AddEmailRecipient(string emailAddress)
        {
            try
            {
                AddRecipient(emailAddress, RecipientType.EmailTo);
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.ToString() );
                return false;
            }

            return true;
        }

        private void AddRecipient(string recipient, RecipientType type)
        {

            switch (type)
            {
                case RecipientType.EmailTo:
                    _destination.Email.To.Add(recipient);
                    break;

                case RecipientType.EmailCC:
                    _destination.Email.Cc.Add(recipient);
                    break;

                case RecipientType.EmailBcc:
                    _destination.Email.Bcc.Add(recipient);
                    break;

                case RecipientType.SMS:
                    _destination.SMS.To.Add(recipient);
                    break;

                case RecipientType.Teams:
                    _destination.Teams.To.Add(recipient);
                    break;
            }
        }

        public void TriggerMessageAsync(string subject, string messageBody)
        {
            var header = new MessageHeader(subject, _destination, _sourceName, _alertLevel, _sourceDefinedId, _sender);
            var message = new MessageSentIntegrationEvent(header, messageBody);

            _eventBus.Publish(message);
        }

        private enum RecipientType
        {
            EmailTo,
            EmailCC,
            EmailBcc,
            SMS,
            Teams
        }
    }

}
