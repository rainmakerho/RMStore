using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using OpenTelemetry.Trace;
using RMStore.Domain;
using RMStore.Infrastructure;
using RMStore.Infrastructure.BaseClasses;

namespace RMStore.WebUI.Pages
{
    public class ProductModel : PageModel
    {
        private readonly ILogger _logger;
        public List<Product> Products;
        private readonly IHttpClientFactory _clientFactory;
        [BindProperty]
        public string ProductName { get; set; } = string.Empty;

        public ProductModel(ILogger<ProductModel> logger
            , IScopeInformation scopeInfo
            , IHttpClientFactory clientFactory)  
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }
        
        public async Task OnGetAsync()
        {
            await GetProducts(ProductName);
        }

        private async Task GetProducts(string productName)
        {
            var tracer = TracerProvider.Default.GetTracer(typeof(Startup).Namespace);
            using var span = tracer.StartSpan("Product.cshtml.GetProducts");
            span?.SetAttribute($"Action:", "GetProducts");


            _logger.LogInformation(message: "RazorPage ENTRY: search products");
                var apiUrl = $"https://localhost:44330/product?productName={HttpUtility.UrlEncode(productName)}";
                if (productName?.ToLower() == "patherror")
                {
                    apiUrl = $"https://localhost:44330/productx?productName={HttpUtility.UrlEncode(productName)}";
                }
                var request = new HttpRequestMessage(HttpMethod.Get,apiUrl);
                var token = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
                request.Headers.Add("Authorization", $"Bearer {token}");
                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);
                var productsJson = await response.Content.ReadAsStringAsync();
                Products = JsonConvert.DeserializeObject<List<Product>>(productsJson)
                        .OrderBy(p => p.ProductID).ToList();

         
                //using (var http = new HttpClient(new StandardHttpMessageHandler(HttpContext, _logger)))
                //{
                //    var apiUrl = $"https://localhost:44330/product?productName={HttpUtility.UrlEncode(productName)}";
                //    if (productName?.ToLower() == "patherror")
                //    {
                //        apiUrl = $"https://localhost:44330/productx?productName={HttpUtility.UrlEncode(productName)}";
                //    }
                //    var response = await http.GetAsync(apiUrl);
                //    var productsJson = await response.Content.ReadAsStringAsync();
                //    Products = JsonConvert.DeserializeObject<List<Product>>(productsJson)
                //            .OrderBy(p => p.ProductID).ToList();
                //}
             
            
        }

        public async Task OnPostAsync()
        {
            await GetProducts(ProductName);
        }
    }
}
