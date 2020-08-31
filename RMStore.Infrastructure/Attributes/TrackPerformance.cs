using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RMStore.Infrastructure.Attributes
{
    public class TrackPerformance : ActionFilterAttribute
    {
        private readonly ILogger<TrackPerformance> _logger;
        private Stopwatch _timmer;
        private readonly IScopeInformation _scopeInfo;
        private IDisposable _userScope;
        private IDisposable _hostScope;

        public TrackPerformance(ILogger<TrackPerformance> logger, IScopeInformation scopeInfo)
        {
            _logger = logger;
            _scopeInfo = scopeInfo;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _timmer = new Stopwatch();
            _timmer.Start();
            _userScope = _logger.BeginScope(_scopeInfo.GetUserScopeInfo(context.HttpContext.User));
            _hostScope = _logger.BeginScope(_scopeInfo.HostScopeInfo);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            _timmer.Stop();
            if (context.Exception == null)
            {
                _logger.LogRoutePerformance(context.HttpContext.Request.Path
                , context.HttpContext.Request.Method
                , _timmer.ElapsedMilliseconds);
            }
            _userScope.Dispose();
            _hostScope.Dispose();
        }
    }
}
