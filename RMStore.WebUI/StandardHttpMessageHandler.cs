using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace RMStore.WebUI
{
    public class StandardHttpMessageHandler : DelegatingHandler
    {
        private readonly HttpContext _httpContext;
        private readonly ILogger _logger;


        public StandardHttpMessageHandler(HttpContext httpContext, ILogger logger)
        {
            _httpContext = httpContext;
            _logger = logger;

            InnerHandler = new SocketsHttpHandler();
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var token = await _httpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            request.Headers.Add("Authorization", $"Bearer {token}");
            var response = await base.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                string errorId = null, errorTitle = null, errorDetail = null;
                if (!string.IsNullOrWhiteSpace(jsonContent))
                {
                    var error = JObject.Parse(jsonContent);

                    if (error != null)
                    {
                        errorId = error["Id"]?.ToString();
                        errorTitle = error["Title"]?.ToString();
                        errorDetail = error["Detail"]?.ToString();
                    }
                }

                var ex = new Exception("API Failure");

                ex.Data.Add("API Route", $"GET {request.RequestUri}");
                ex.Data.Add("API Status", (int)response.StatusCode);
                ex.Data.Add("API ErrorId", errorId);
                ex.Data.Add("API Title", errorTitle);
                ex.Data.Add("API Detail", errorDetail);

                _logger.LogWarning("API Error when calling {APIRoute}: {APIStatus}," +
                    " {ApiErrorId} - {Title} - {Detail}",
                    $"GET {request.RequestUri}", (int)response.StatusCode,
                    errorId, errorTitle, errorDetail);
                throw ex;
            }
            return response;
        }
    }
}
