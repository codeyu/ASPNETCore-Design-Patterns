using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Layered.Model
{
    public class NullDiscountStrategy : IDiscountStrategy 
    {        
        public decimal ApplyExtraDiscountsTo(decimal OriginalSalePrice)
        {
            return OriginalSalePrice;
        }
    }
}
