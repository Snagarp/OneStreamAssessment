//2024 (c) TD Synnex - All Rights Reserved.

using Configuration.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace Common.EventBus.NotificationMessage
{
    public class MessageDistributionMethod : Enumeration
    {
        public static readonly MessageDistributionMethod email = new(1, nameof(email).ToUpperInvariant());
        public static readonly MessageDistributionMethod sms = new(2, nameof(sms).ToUpperInvariant());
        public static readonly MessageDistributionMethod teams= new(3, nameof(teams).ToUpperInvariant());

        public MessageDistributionMethod(int id, string name) : base(id, name)
        {
        }
    }

    public class Destination
    {
        public Destination()
        {
            Email = new EmailRecipient();
            SMS = new SMSRecipient();
            Teams = new TeamsRecipient();
        }

        [JsonPropertyName("email")]
        public EmailRecipient? Email { get; set; }
        [JsonPropertyName("sms")]
        public SMSRecipient? SMS { get; set; }
        [JsonPropertyName("teams")]
        public TeamsRecipient? Teams { get; set; }
    }

    public class EmailRecipient: RecipientDistribution
    {
        public EmailRecipient()
        {
            To = new List<string>();
            Cc = new List<string>();
            Bcc = new List<string>();
        }

        [JsonPropertyName("cc")]
        public List<string>? Cc { get; set; }
        [JsonPropertyName("bcc")]
        public List<string>? Bcc { get; set; }
    }

    public class SMSRecipient : RecipientDistribution
    {
        public SMSRecipient()
        {
            To = new List<string>();
        }
    }

    public class TeamsRecipient : RecipientDistribution
    {
        public TeamsRecipient()
        {
            To = new List<string>();
        }
    }

    public class RecipientDistribution
    {
        [JsonPropertyName("to")]
        public List<string> To { get; set; }
    }
}
