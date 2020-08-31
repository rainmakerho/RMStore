using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RMStore.Infrastructure.Filters
{
    public class TrackActionPerformanceFilter : IActionFilter
    {
        private readonly ILogger<TrackActionPerformanceFilter> _logger;
        private Stopwatch _timmer;
        private readonly IScopeInformation _scopeInfo;
        private IDisposable _userScope;
        private IDisposable _hostScope;

        public TrackActionPerformanceFilter(ILogger<TrackActionPerformanceFilter> logger,
            IScopeInformation scopeInfo)
        {
            _logger = logger;
            _scopeInfo = scopeInfo;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _timmer = new Stopwatch();
            _timmer.Start();
            _userScope = _logger.BeginScope(_scopeInfo.GetUserScopeInfo(context.HttpContext.User));
            _hostScope = _logger.BeginScope(_scopeInfo.HostScopeInfo);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _timmer.Stop();
            if (context.Exception == null)
            {
                _logger.LogRoutePerformance(context.HttpContext.Request.Path
                , context.HttpContext.Request.Method
                , _timmer.ElapsedMilliseconds);
            }
            _userScope?.Dispose();
            _hostScope?.Dispose();
        }

        
    }
}
