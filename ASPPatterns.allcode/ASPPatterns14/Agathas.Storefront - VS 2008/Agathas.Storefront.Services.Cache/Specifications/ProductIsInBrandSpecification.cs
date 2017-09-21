using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agathas.Storefront.Model.Products;

namespace Agathas.Storefront.Services.Cache.Specifications
{
    public class ProductIsInBrandSpecification : IProductSearchSpecification 
    {
        private readonly int[] _brandIds;

        public ProductIsInBrandSpecification(int[] brandIds)
        {
            _brandIds = brandIds;            
        }

        public bool IsSatisfiedBy(Product product)
        {
            if (_brandIds.Count() > 0)
                return _brandIds.Any(b => b == product.Title.Brand.Id);
            
            return true;
        }
    }
}
