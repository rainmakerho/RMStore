using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            using(_logger.BeginScope("Database Access"))
            {
                _logger.LogInformation(message: "GetAllProducts({productName})", productName);
                var collection = _dbContext.Products as IQueryable<Product>;
                if (!string.IsNullOrWhiteSpace(productName))
                {
                    collection = collection.Where(
                            p => p.Name.ToLower().Contains(productName.ToLower()));
                }
                if (productName == "error")
                    throw SqlExceptionCreator.NewSqlException();
                if (productName == "error2")
                    throw SqlExceptionCreator.NewSqlException(2);
                return collection.ToList();
            }
            
        }

    }
}
