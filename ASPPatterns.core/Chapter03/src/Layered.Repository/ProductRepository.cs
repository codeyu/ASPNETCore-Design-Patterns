using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Layered.Model;   

namespace Layered.Repository
{
    public class ProductRepository : IProductRepository 
    {        
        public IList<Model.Product> FindAll()
        {
            var products = from p in new ShopDataContext().Products
                           select new Model.Product
                               {
                                   Id = p.ProductId, 
                                   Name = p.ProductName,                                   
                                   Price = new Model.Price(p.RRP, p.SellingPrice)                                                                        
                               };            

            return products.ToList();
        }
     
    }
}
