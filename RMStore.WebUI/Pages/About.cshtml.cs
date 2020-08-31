using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RMStore.Infrastructure;
using RMStore.Infrastructure.BaseClasses;

namespace RMStore.WebUI.Pages
{
    public class AboutModel : PageModel
    {

        public AboutModel(ILogger<AboutModel> logger, IScopeInformation scopeInfo) 
        {}

        public void OnGet()
        {
            throw new Exception("this is exception from about page...");
        }
    }
}
