namespace Common.Http
{
    /// <summary>
    /// Defines methods to help with making HTTP requests such as POST and GET, and setting the media type for requests.
    /// </summary>
    public interface IHttpClientHelper
    {
        /// <summary>
        /// Sends an HTTP POST request asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the request body.</typeparam>
        /// <param name="apiUrl">The URL of the API to send the POST request to.</param>
        /// <param name="postRequest">The request body to send in the POST request.</param>
        /// <param name="requestId">An identifier for tracking the request.</param>
        /// <returns>A dynamic result, typically the response from the API.</returns>
        Task<dynamic> PostAsync<T>(string apiUrl, T postRequest, string requestId);

        /// <summary>
        /// Sends an HTTP GET request asynchronously.
        /// </summary>
        /// <param name="apiUrl">The URL of the API to send the GET request to.</param>
        /// <returns>A dynamic result, typically the response from the API.</returns>
        Task<dynamic> GetAsync(string apiUrl);

        /// <summary>
        /// Sets the media type for the HTTP requests (e.g., "application/json").
        /// </summary>
        /// <param name="mediaType">The media type to set for the requests.</param>
        void SetMediaType(string mediaType);
    }
}
