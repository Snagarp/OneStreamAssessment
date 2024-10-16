using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;

namespace Identity.Security
{
    internal class HttpClientService
    {
        private static readonly IDictionary<string, HttpClient> Cache = new ConcurrentDictionary<string, HttpClient>();

        internal HttpClientService()
        {
        }

        internal static HttpClient Create(string endpoint)
        {

            string cacheKey = $"{endpoint}";

            if (Cache.TryGetValue(cacheKey, out HttpClient client))
            {
                return client;
            }

            client = new HttpClient
            {
                BaseAddress = new Uri(endpoint)
            };

            Cache[cacheKey] = client;

            return client;

        }
    }
}
