using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Layered.Model
{
    public interface IDiscountStrategy
    {
        decimal ApplyExtraDiscountsTo(decimal OriginalSalePrice);
    }
}
