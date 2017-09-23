using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Service; 

namespace Tests.Stubs
{
    public class StubProductRepository : IProductRepository 
    {        
        public IList<Product> GetAllProductsIn(int categoryId)
        {
            IList<Product> products = new List<Product>();            

            return products;
        }
    }
}
