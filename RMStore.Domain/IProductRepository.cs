using System;
using System.Collections.Generic;
using System.Text;

namespace RMStore.Domain
{
    public interface IProductRepository
    {
        List<Product> GetAllProducts(string productName);

    }
}
