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
        private readonly InMemoryDbContext _dbContext;

        public ProductController(ILogger<ProductController> logger, InMemoryDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet()]
        [HttpHead]
        public ActionResult<IEnumerable<Product>> GetProducts(
            [FromQuery] string productName)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            _logger.LogInformation(message: "{userId} is inside get all products API call. {Claims}",
                User.Identity.Name, userId, User.Claims);
            var collection = _dbContext.Products as IQueryable<Product>;
            if (!string.IsNullOrWhiteSpace(productName))
            {
                collection = collection.Where(
                        p=>p.Name.Contains(productName));
            }
            return Ok(collection.ToList());
        }

        [HttpOptions]
        public IActionResult GetProductsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }

    }
}
