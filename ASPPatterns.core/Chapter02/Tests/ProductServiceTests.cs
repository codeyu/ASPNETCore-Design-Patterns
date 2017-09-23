using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Service;
using Tests.Stubs;
using Tests.Mocks;

namespace Tests
{
    public class ProductServiceTests
    {
        [Fact]        
        public void A_ProductService_Should_Retrieve_From_The_Cache_The_Second_Time_The_Method_Is_Called_With_The_Same_Argumengts()
        {
            MockCacheStorage mockCacheStorage = new MockCacheStorage();
            StubProductRepository stubProductRepository = new StubProductRepository();
            int categoryId = 1;
            ProductService productService = new ProductService(stubProductRepository, mockCacheStorage);

            productService.GetAllProductsIn(categoryId);
            Assert.Equal(0, mockCacheStorage.RetrievedFromCacheCount());

            productService.GetAllProductsIn(categoryId);
            Assert.Equal(1, mockCacheStorage.RetrievedFromCacheCount());

        }

        [Fact]
        public void A_Null_Object_Caching_Adapter_Will_Prevent_Any_Data_Caching()
        {
            MockProductRepository mockProductRepository = new MockProductRepository();
            NullObjectCachingAdapter nullObjectCachingAdapter = new NullObjectCachingAdapter();
            int categoryId = 1;
            ProductService productService = new ProductService(mockProductRepository, nullObjectCachingAdapter);

            productService.GetAllProductsIn(categoryId);
            Assert.Equal(1, mockProductRepository.NumberOfTimesCalled());

            productService.GetAllProductsIn(categoryId);
            Assert.Equal(2, mockProductRepository.NumberOfTimesCalled());
        }
    }
}
