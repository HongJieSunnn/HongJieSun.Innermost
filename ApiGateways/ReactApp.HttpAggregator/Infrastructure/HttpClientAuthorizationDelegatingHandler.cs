// Copyright (c) .NET Foundation and Contributors. All Rights Reserved.
//
// Distributed under MIT license.
// See file LICENSE for detail or copy at https://opensource.org/licenses/MIT

// Framework code of microservices and domain drive design pattern

//Modified by HongJieSun 2022
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace ReactApp.HttpAggregator.Infrastructure
{
    /// <summary>
    /// Use for add authroization header for http requests call from HttpClient.
    /// </summary>
    public class HttpClientAuthorizationDelegatingHandler
        : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpClientAuthorizationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authorizationHeader = _httpContextAccessor.HttpContext!.Request.Headers["Authorization"];
            var xrequestIdHeader = _httpContextAccessor.HttpContext!.Request.Headers["x-requestid"];

            if (!string.IsNullOrWhiteSpace(authorizationHeader))
            {
                request.Headers.Add("Authorization", new List<string>() { authorizationHeader });
            }

            if (string.IsNullOrWhiteSpace(xrequestIdHeader))
            {
                request.Headers.Add("x-requestid", Guid.NewGuid().ToString());
            }

            var token = await GetTokenAsync();

            if (token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }

        Task<string?> GetTokenAsync()
        {
            const string ACCESS_TOKEN = "access_token";

            return _httpContextAccessor.HttpContext!.GetTokenAsync(ACCESS_TOKEN);
        }
    }
}
