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
        private readonly IScopeInformation _scopeInfo;
        private IDisposable _userScope;
        private IDisposable _hostScope;
        public BasePageModel(ILogger logger, IScopeInformation scopeInfo)
        {
            _logger = logger;
            _scopeInfo = scopeInfo;
        }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            _timmer = new Stopwatch();
            _timmer.Start();
            _userScope = _logger.BeginScope(_scopeInfo.GetUserScopeInfo(context.HttpContext.User));
            _hostScope = _logger.BeginScope(_scopeInfo.HostScopeInfo);
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
            _userScope.Dispose();
            _hostScope.Dispose();
        }
    }
}
