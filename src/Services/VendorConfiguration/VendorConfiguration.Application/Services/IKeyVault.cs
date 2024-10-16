namespace VendorConfiguration.Application.Services
{
    public interface IKeyVault
    {
        Task<string?> GetSecret(string key);
        Task<dynamic?> GetSecretAsJson(string key);
    }
}
