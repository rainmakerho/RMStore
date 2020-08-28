using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RMStore.Infrastructure.Filters
{
    public class TrackActionPerformanceFilter : IActionFilter
    {
        private readonly ILogger<TrackActionPerformanceFilter> _logger;
        private Stopwatch _timmer;

        public TrackActionPerformanceFilter(ILogger<TrackActionPerformanceFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _timmer = new Stopwatch();
            _timmer.Start();
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
        }

        
    }
}
