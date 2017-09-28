using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Layered.Service; 

namespace Layered.Presentation
{
    public class ProductListPresenter
    {
        private IProductListView _productListView;
        private Service.ProductService _productService;
                
        public ProductListPresenter(IProductListView ProductListView, Service.ProductService ProductService)
        {
            _productService = ProductService;
            _productListView = ProductListView;
        }

        public void Display()
        {
            ProductListRequest productListRequest = new ProductListRequest();
            productListRequest.CustomerType = _productListView.CustomerType;

            ProductListResponse productResponse = _productService.GetAllProductsFor(productListRequest);

            if (productResponse.Success)
            {
                _productListView.Display(productResponse.Products);
            }
            else 
            {
                _productListView.ErrorMessage = productResponse.Message; 
            }
   
        }
    }
}
