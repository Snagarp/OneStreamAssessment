using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Common.Http
{
    /// <summary>
    /// Provides helper methods for making HTTP requests with support for headers from the current HTTP context.
    /// </summary>
    public class HttpClientHelper : IHttpClientHelper
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _mediaType = "application/json";

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientHelper"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor used to retrieve HTTP headers.</param>
        public HttpClientHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = new HttpClient();
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Sets the media type for the HTTP requests (e.g., "application/json", "application/xml").
        /// </summary>
        /// <param name="mediaType">The media type to set.</param>
        public void SetMediaType(string mediaType) => _mediaType = mediaType;

        /// <summary>
        /// Sends an HTTP POST request to the specified API URL with the given payload.
        /// </summary>
        /// <typeparam name="T">The type of the request payload.</typeparam>
        /// <param name="apiUrl">The API URL to send the request to.</param>
        /// <param name="postRequest">The payload to be sent in the request.</param>
        /// <param name="requestId">The request ID for tracking purposes.</param>
        /// <returns>The response as a dynamic object.</returns>
        public async Task<dynamic> PostAsync<T>(string apiUrl, T postRequest, string requestId)
        {
            SetRequestId(requestId);
            SetAuthorizationHeaders();

            HttpResponseMessage response;
            switch (postRequest)
            {
                case MultipartFormDataContent multipartContent:
                    response = await _httpClient.PostAsync(apiUrl, multipartContent);
                    break;
                default:
                {
                    string request = _mediaType == "application/xml"
                        ? postRequest?.ToString() ?? string.Empty
                        : JsonConvert.SerializeObject(postRequest);
                    var content = new StringContent(request, System.Text.Encoding.UTF8, _mediaType);
                    response = await _httpClient.PostAsync(apiUrl, content);
                    break;
                }
            }

            return await GetResponse(response);
        }

        /// <summary>
        /// Sends an HTTP GET request to the specified API URL.
        /// </summary>
        /// <param name="apiUrl">The API URL to send the request to.</param>
        /// <returns>The response as a dynamic object.</returns>
        public async Task<dynamic> GetAsync(string apiUrl)
        {
            SetAuthorizationHeaders();
            var response = await _httpClient.GetAsync(apiUrl);
            return await GetResponse(response);
        }

        /// <summary>
        /// Sets authorization headers from the current HTTP context.
        /// </summary>
        private void SetAuthorizationHeaders()
        {
            var headers = _httpContextAccessor.HttpContext?.Request?.Headers;
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    if (header.Key.Equals("Authorization", StringComparison.InvariantCultureIgnoreCase) ||
                        header.Key.Equals("JwtToken", StringComparison.InvariantCultureIgnoreCase))
                    {
                        _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Generates a new request ID if none is provided.
        /// </summary>
        /// <returns>A new request ID.</returns>
        private static string GetRequestId() => Guid.NewGuid().ToString();

        /// <summary>
        /// Sets the request ID header for the HTTP client.
        /// </summary>
        /// <param name="requestId">The request ID to set.</param>
        private void SetRequestId(string requestId)
        {
            requestId = string.IsNullOrEmpty(requestId) ? GetRequestId() : requestId;
            _httpClient.DefaultRequestHeaders.Add("x-requestid", requestId);
        }

        /// <summary>
        /// Processes the response from the HTTP request.
        /// </summary>
        /// <param name="response">The HTTP response message.</param>
        /// <returns>The response as a dynamic object.</returns>
        private static async Task<dynamic> GetResponse(HttpResponseMessage response)
        {
            var responseString = await response.Content.ReadAsStringAsync();

            var contentType = response.Content.Headers.ContentType?.MediaType;

            if (string.Equals(contentType, "application/xml", StringComparison.OrdinalIgnoreCase))
            {
                return responseString;
            }
            else if (string.Equals(contentType, "text/csv", StringComparison.OrdinalIgnoreCase))
            {
                return response;
            }
            else
            {
                return System.Text.Json.JsonSerializer.Deserialize<JsonNode>(
                    responseString,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                ) ?? new JsonObject(); 
            }
        }
    }
}
