using System;
using System.Collections.Generic;

namespace Service
{
    public interface IProductRepository
    {
        IList<Product> GetAllProductsIn(int categoryId);
    }
}
