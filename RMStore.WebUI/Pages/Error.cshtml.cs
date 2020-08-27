using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace RMStore.WebUI.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {
        private readonly ILogger<ErrorModel> _logger;

        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string ApiRoute { get; set; }

        public string ApiStatus { get; set; }

        public string ApiErrorId { get; set; }

        public string ApiTitle { get; set; }

        public string ApiDetail { get; set; }



        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        //https://stackoverflow.com/questions/57954352/razor-custom-error-page-ignores-its-onget-handler

        public void OnGet()
        {
            ProcessError();
        }

        public void OnPost()
        {
            ProcessError();
        }

        private void ProcessError()
        {
            _logger.LogInformation("Error Page OnGet");
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            var exceptionPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var ex = exceptionPathFeature?.Error;
            if (ex !=null && ex.Data.Contains("API Route"))
            {
                ApiRoute = ex.Data["API Route"]?.ToString();
                ApiStatus = ex.Data["API Status"]?.ToString();
                ApiErrorId = ex.Data["API ErrorId"]?.ToString();
                ApiTitle = ex.Data["API Title"]?.ToString();
                ApiDetail = ex.Data["API Detail"]?.ToString();


            }
        }
    }
}
