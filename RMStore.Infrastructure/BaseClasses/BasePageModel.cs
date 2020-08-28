using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RMStore.Infrastructure.BaseClasses
{
    public class BasePageModel : PageModel
    {
        private readonly ILogger _logger;
        private Stopwatch _timmer;

        public BasePageModel(ILogger logger)
        {
            _logger = logger;
        }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            _timmer = new Stopwatch();
            _timmer.Start();
        }

        public override void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            _timmer.Stop();
            if(context.Exception == null)
            {
                _logger.LogRoutePerformance(context.ActionDescriptor.RelativePath
                    , context.HttpContext.Request.Method
                    , _timmer.ElapsedMilliseconds);
            }
            
        }
    }
}
