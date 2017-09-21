using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agathas.Storefront.Model.Products;

namespace Agathas.Storefront.Services.Cache.Specifications
{
    public class ProductIsInCategorySpecification : IProductSearchSpecification
    {
        private readonly int _categoryId;

        public ProductIsInCategorySpecification(int categoryId)
        {
            _categoryId = categoryId;
        }

        public bool IsSatisfiedBy(Product product)
        {
            return product.Category.Id == _categoryId;            
        }
    }
}
