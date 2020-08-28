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

        public TrackPerformance(ILogger<TrackPerformance> logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _timmer = new Stopwatch();
            _timmer.Start();
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
        }
    }
}
