namespace Identity.Security.Models
{
    internal interface IIdpResponse
    {
        string Uuid { get; set; }

        string Username { get; set; }

        string TokenType { get; set; }

        string ExpiresIn { get; set; }

        string Scope { get; set; }

        string ClientId { get; }
    }
}
