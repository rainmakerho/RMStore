﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RMStore.Infrastructure.Filters
{
    public class TrackPagePerformanceFilter : IPageFilter
    {
        private readonly ILogger<TrackPagePerformanceFilter> _logger;
        private Stopwatch _timmer;

        public TrackPagePerformanceFilter(ILogger<TrackPagePerformanceFilter> logger)
        {
            _logger = logger;
        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            _timmer = new Stopwatch();
            _timmer.Start();
        }


        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            _timmer.Stop();
            if(context.Exception == null)
            {
                _logger.LogRoutePerformance(context.ActionDescriptor.RelativePath
                , context.HttpContext.Request.Method
                , _timmer.ElapsedMilliseconds);
            }
            
        }

        
        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
            //do nothing
        }
    }
}
