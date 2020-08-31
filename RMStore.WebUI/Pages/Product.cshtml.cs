using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RMStore.Domain;
using RMStore.Infrastructure;
using RMStore.Infrastructure.BaseClasses;

namespace RMStore.WebUI.Pages
{
    public class ProductModel : PageModel
    {
        private readonly ILogger _logger;
        public List<Product> Products;

        [BindProperty]
        public string ProductName { get; set; } = string.Empty;

        public ProductModel(ILogger<ProductModel> logger, IScopeInformation scopeInfo)  
        {
            _logger = logger;
        }
        
        public async Task OnGetAsync()
        {
            await GetProducts(ProductName);
        }

        private async Task GetProducts(string productName)
        {
            _logger.LogInformation(message: "RazorPage ENTRY: search products");
            using (var http = new HttpClient(new StandardHttpMessageHandler(HttpContext, _logger)))
            {
                var apiUrl = $"https://localhost:44330/product?productName={HttpUtility.UrlEncode(productName)}";
                if (productName.ToLower() == "patherror")
                {
                    apiUrl = $"https://localhost:44330/productx?productName={HttpUtility.UrlEncode(productName)}";
                }
                var response = await http.GetAsync(apiUrl);
                var productsJson = await response.Content.ReadAsStringAsync();
                Products = JsonConvert.DeserializeObject<List<Product>>(productsJson)
                        .OrderBy(p => p.ProductID).ToList();
            }
        }

        public async Task OnPostAsync()
        {
            await GetProducts(ProductName);
        }
    }
}
