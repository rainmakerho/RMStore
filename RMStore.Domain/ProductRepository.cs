using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMStore.Domain
{
    public class ProductRepository : IProductRepository
    {
        private readonly InMemoryDbContext _dbContext;
        private readonly ILogger<ProductRepository> _logger;
        public ProductRepository(InMemoryDbContext dbContext, ILogger<ProductRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public List<Product> GetAllProducts(string productName)
        {
            _logger.LogInformation(message: "在 Repository 中依產品名稱取得資料");
            var collection = _dbContext.Products as IQueryable<Product>;
            if (!string.IsNullOrWhiteSpace(productName))
            {
                collection = collection.Where(
                        p => p.Name.Contains(productName));
            }
            return collection.ToList();
        }
    }
}
