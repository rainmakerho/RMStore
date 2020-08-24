using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RMStore.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public ProductController(ILogger<ProductController> logger, IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        [HttpGet()]
        [HttpHead]
        public ActionResult<IEnumerable<Product>> GetProducts(
            [FromQuery] string productName)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            _logger.LogInformation(message: "{userId} is inside get all products API call. {Claims}",
                User.Identity.Name, userId, User.Claims);
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
