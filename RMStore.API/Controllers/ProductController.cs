using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;
using RMStore.Domain;
using RMStore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RMStore.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly ILogger<ProductController> _logger;
        private readonly IProductRepository _productRepository;
        private readonly IScopeInformation _scopeInformation;

        public ProductController(ILogger<ProductController> logger, IProductRepository productRepository
            , IScopeInformation scopeInformation)
        {
            _logger = logger;
            _productRepository = productRepository;
            _scopeInformation = scopeInformation;
        }

        [HttpGet()]
        [HttpHead]
        public ActionResult<IEnumerable<Product>> GetProducts(
            [FromQuery] string productName)
        {
            var tracer = TracerProvider.Default.GetTracer(typeof(Startup).Namespace);
            using var span = tracer.StartSpan("API.GetProducts");
            span.SetAttribute($"Action:", "ProductAPI.GetProducts");
            span?.SetAttribute($"productName", productName);
            _logger.LogInformation(message: "API ENTRY: Inside search products API Call");
            var products = _productRepository.GetAllProducts(productName);
            return Ok(products);
        }

        [HttpOptions]
        public IActionResult GetProductsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }

    }
}
