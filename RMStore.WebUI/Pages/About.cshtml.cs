using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RMStore.Infrastructure.BaseClasses;

namespace RMStore.WebUI.Pages
{
    public class AboutModel : BasePageModel
    {

        public AboutModel(ILogger<AboutModel> logger) : base(logger)
        {}

        public void OnGet()
        {
            throw new Exception("this is exception from about page...");
        }
    }
}
