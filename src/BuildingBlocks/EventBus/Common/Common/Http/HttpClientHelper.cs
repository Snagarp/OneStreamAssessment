//2023 (c) TD Synnex - All Rights Reserved.



using Identity.Security.OAuth.Abstract;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Common.Http
{
    public class HttpClientHelper : IHttpClientHelper
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _mediaType = "application/json";

        public HttpClientHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = new HttpClient();
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetMediaType(string mediatType)
        {
            _mediaType = mediatType;
        }

        public async Task<dynamic> PostAsync<T>(string apiUrl, T postRequest, string requestId)
        {

            SetRequestId(requestId);
            SetAuthorizationHeaders();
            if (postRequest is MultipartFormDataContent)
            {
                var response = await _httpClient.PostAsync(apiUrl, postRequest as MultipartFormDataContent);
                return await GetResponse(response);
            }
            else
            {
                string request = (_mediaType == "application/xml") ? postRequest.ToString() : JsonConvert.SerializeObject(postRequest);
                var content = new StringContent(request, System.Text.Encoding.UTF8, _mediaType);
                var response = await _httpClient.PostAsync(apiUrl, content);
                return await GetResponse(response);
            }
        }

        public async Task<dynamic> GetAsync(string apiUrl)
        {
            SetAuthorizationHeaders();
            var response = await _httpClient.GetAsync(apiUrl);
            return await GetResponse(response);
        }

        private void SetAuthorizationHeaders()
        {
            if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.Request != null && _httpContextAccessor.HttpContext.Request.Headers != null)
            {
                foreach (var header in _httpContextAccessor.HttpContext.Request.Headers)
                {
                    if (header.Key.Equals("Authorization", StringComparison.InvariantCultureIgnoreCase)
                       || header.Key.Equals("JwtToken", StringComparison.InvariantCultureIgnoreCase)
                       )
                        _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value.ToString());
                }
            }
        }
        private string GetRequestId()
        {
            return Guid.NewGuid().ToString();
        }

        private void SetRequestId(string requestId)
        {
            requestId = string.IsNullOrEmpty(requestId) ? requestId : GetRequestId();
            _httpClient.DefaultRequestHeaders.Add("x-requestid", requestId);
        }

        private async Task<dynamic> GetResponse(HttpResponseMessage response)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            if (response.Content.Headers.ContentType.MediaType.Equals("application/xml"))
            {
                return responseString;
            }
            else if (response.Content.Headers.ContentType.MediaType.Equals("text/csv"))
            {
                return response;
            }
            else
            {
                var jsonResponse = System.Text.Json.JsonSerializer.Deserialize<JsonNode>(responseString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return jsonResponse;
            }
        }
    }

}
