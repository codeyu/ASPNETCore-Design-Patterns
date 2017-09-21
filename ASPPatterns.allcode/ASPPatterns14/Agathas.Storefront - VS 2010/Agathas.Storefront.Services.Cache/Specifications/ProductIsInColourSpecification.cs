using System;
using System.Linq;
using Agathas.Storefront.Model.Products;

namespace Agathas.Storefront.Services.Cache.Specifications
{
    public class ProductIsInColourSpecification : IProductSearchSpecification
    {
        private readonly int[] _colourIds;

        public ProductIsInColourSpecification(int[] colourIds)
        {
            _colourIds = colourIds;
        }

        public bool IsSatisfiedBy(Product product)
        {
            if (_colourIds.Count() > 0)
                return _colourIds.Any(c => c == product.Title.Color.Id);
            
            return true;
        }
    }
}