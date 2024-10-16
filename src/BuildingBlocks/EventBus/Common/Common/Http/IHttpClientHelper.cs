//2023 (c) TD Synnex - All Rights Reserved.



namespace Common.Http
{
    public interface IHttpClientHelper
    {
        Task<dynamic> PostAsync<T>(string apiUrl, T postRequest, string requestId);
        Task<dynamic> GetAsync(string apiUrl);
        void SetMediaType(string mediatType);
    }
}
