using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace RMStore.Infrastructure.Middleware
{
    public class ApiExceptionMiddleware
    {
        private readonly ApiExceptionOptions _options;
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiExceptionMiddleware> _logger;
        private readonly IScopeInformation _scopeInfo;
        public ApiExceptionMiddleware(ApiExceptionOptions options, RequestDelegate next
            , ILogger<ApiExceptionMiddleware> logger, IScopeInformation scopeInfo)
        {
            _options = options;
            _next = next;
            _logger = logger;
            _scopeInfo = scopeInfo;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var error = new ApiError
            {
                Id = Guid.NewGuid().ToString(),
                Status = (short)HttpStatusCode.InternalServerError,
                Title = "api 發生錯誤，請洽管理人員"
            };
            _options.AddResponseDetails?.Invoke(context, exception, error);
            var level = _options.DetermineLogLevel?.Invoke(exception) ?? LogLevel.Error;
            var innerExMessage = GetInnermostExceptionMessage(exception);

            using (_logger.BeginScope(_scopeInfo.GetUserScopeInfo(context.User)))
            using (_logger.BeginScope(_scopeInfo.HostScopeInfo))
            {
                //_logger.LogError(exception, "api 發生錯誤!!! " + innerExMessage + " --{ErrorId}.", error.Id);
                _logger.Log(level, exception, "api 發生錯誤!!! " + innerExMessage + " --{ErrorId}.", error.Id);
            }
            var result = JsonConvert.SerializeObject(error);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(result);
        }

        private string GetInnermostExceptionMessage(Exception exception)
        {
            if (exception.InnerException != null)
                return GetInnermostExceptionMessage(exception.InnerException);
            return exception.Message;
        }
    }
}
