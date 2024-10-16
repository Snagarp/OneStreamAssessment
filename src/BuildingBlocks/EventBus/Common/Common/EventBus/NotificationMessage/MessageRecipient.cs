
namespace Common.EventBus.NotificationMessage
{
    public class MessageRecipient
    {
        public MessageDistributionMethod DistributionMethod { get; set; }
        public string DistrubtionAddress { get; set; }

        public MessageRecipient(MessageDistributionMethod distMethod, string distAddress) 
        { 
            DistributionMethod = distMethod;
            DistrubtionAddress = distAddress;
        }
    }
}
