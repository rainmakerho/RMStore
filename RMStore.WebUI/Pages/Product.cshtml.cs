using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RMStore.Domain;

namespace RMStore.WebUI.Pages
{
    public class ProductModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        public List<Product> Products;

        public ProductModel(ILogger<ProductModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        
        public async Task OnGetAsync()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            _logger.LogInformation(message: "{UserName}({userId}) is about to call the product api go get all products. {Claims}",
                User.Identity.Name, userId, User.Claims);
            using (var http = new HttpClient(new StandardHttpMessageHandler(HttpContext, _logger)))
            {
                var response = await http.GetAsync("https://localhost:44330/product");
                var productsJson = await response.Content.ReadAsStringAsync();
                Products = JsonConvert.DeserializeObject<List<Product>>(productsJson)
                        .OrderBy(p => p.ProductID).ToList();
            }
        }
    }
}
