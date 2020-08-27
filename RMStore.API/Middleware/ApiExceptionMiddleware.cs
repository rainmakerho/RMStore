using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RMStore.API.Middleware
{
    public class ApiExceptionMiddleware
    {
        private readonly ApiExceptionOptions _options;
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiExceptionMiddleware> _logger;
        public ApiExceptionMiddleware(ApiExceptionOptions options, RequestDelegate next
            , ILogger<ApiExceptionMiddleware> logger)
        {
            _options = options;
            _next = next;
            _logger = logger;
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

            var innerExMessage = GetInnermostExceptionMessage(exception);

            _logger.LogError(exception, "api 發生錯誤!!! " + innerExMessage + " --{ErrorId}.", error.Id);

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
