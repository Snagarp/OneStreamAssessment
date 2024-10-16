//2023 (c) TD Synnex - All Rights Reserved.

using Common.Validation;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

using System.Net.Http.Headers;


namespace Common.Http
{
    public class HttpClientAuthorizationDelegatingHandler
         : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpClientAuthorizationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = ArgumentGuard.NotNull(httpContextAccessor);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ArgumentGuard.NotNull(request, nameof(request));
            ArgumentGuard.NotNull(cancellationToken, nameof(cancellationToken));

            var headers = _httpContextAccessor!.HttpContext!.Request?.Headers;
            if (headers is not null)
            {
                var authorizationHeader = headers["Authorization"];
                if (!string.IsNullOrWhiteSpace(authorizationHeader))
                {
                    request.Headers.Add("Authorization", new List<string>() { authorizationHeader! });
                }
            }

            var token = await GetTokenAsync().ConfigureAwait(false);

            if (token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        async Task<string?> GetTokenAsync()
        {
            const string ACCESS_TOKEN = "access_token";

            return await _httpContextAccessor!
                .HttpContext!
                .GetTokenAsync(ACCESS_TOKEN).ConfigureAwait(false);
        }
    }
}
