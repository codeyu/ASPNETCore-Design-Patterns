using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agathas.Storefront.Model.Products;

namespace Agathas.Storefront.Services.Cache.Specifications
{
    public interface IProductSearchSpecification
    {
        bool IsSatisfiedBy(Product product);
    }
}
