using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agathas.Storefront.Model.Products;
using Agathas.Storefront.Services.Cache.CacheStorage;
using Agathas.Storefront.Services.Cache.Specifications;
using Agathas.Storefront.Services.Interfaces;
using Agathas.Storefront.Services.Mapping;
using Agathas.Storefront.Services.Messaging.ProductCatalogService;
using Agathas.Storefront.Services.ViewModels;

namespace Agathas.Storefront.Services.Cache
{
    public class CachedProductCatalogService : IProductCatalogService
    {
        private readonly ICacheStorage _cachStorage;
        private readonly IProductCatalogService _realProductCatalogService;
        private readonly IProductTitleRepository _productTitleRepository;
        private readonly IProductRepository _productRepository;
        private readonly Object _getTopSellingProductsLock = new Object();
        private readonly Object _getAllProductTitlesLock = new Object();
        private readonly Object _getAllProductsLock = new Object();
        private readonly object _getAllCategoriesLock = new object();

        public CachedProductCatalogService(ICacheStorage cachStorage,
                                             IProductCatalogService realProductCatalogService,
                                             IProductTitleRepository productTitleRepository,
                                             IProductRepository productRepository)
        {
            _cachStorage = cachStorage;
            _realProductCatalogService = realProductCatalogService;
            _productTitleRepository = productTitleRepository;
            _productRepository = productRepository;
        }

        private IEnumerable<ProductTitle> FindAllProductTitles()
        {
            lock (_getAllProductTitlesLock)
            {
                IEnumerable<ProductTitle> allProductTitles;

                allProductTitles =
                    _cachStorage.Retrieve<IEnumerable<ProductTitle>>(CacheKeys.AllProductTitles.ToString());

                if (allProductTitles == null)
                {
                    allProductTitles = _productTitleRepository.FindAll();
                    _cachStorage.Store(CacheKeys.AllProductTitles.ToString(), allProductTitles);
                }

                return allProductTitles;
            }
        }

        private IEnumerable<Product> FindAllProducts()
        {
            lock (_getAllProductsLock)
            {
                IEnumerable<Product> allProducts;

                allProducts = _cachStorage.Retrieve<IEnumerable<Product>>(CacheKeys.AllProducts.ToString());

                if (allProducts == null)
                {
                    allProducts = _productRepository.FindAll();
                    _cachStorage.Store(CacheKeys.AllProducts.ToString(), allProducts);
                }

                return allProducts;
            }
        }

        public GetFeaturedProductsResponse GetFeaturedProducts()
        {
            lock (_getTopSellingProductsLock)
            {
                GetFeaturedProductsResponse response = new GetFeaturedProductsResponse();
                IEnumerable<ProductSummaryView> productViews;

                productViews =
                    _cachStorage.Retrieve<IEnumerable<ProductSummaryView>>(CacheKeys.TopSellingProducts.ToString());

                if (productViews == null)
                {
                    response = _realProductCatalogService.GetFeaturedProducts();
                    _cachStorage.Store(CacheKeys.TopSellingProducts.ToString(), response.Products);
                }
                else
                {
                    response.Products = productViews;
                }

                return response;
            }
        }

        public GetProductsByCategoryResponse GetProductsByCategory(GetProductsByCategoryRequest request)
        {
            IProductSearchSpecification colourSpecification =
                new ProductIsInColourSpecification(request.ColorIds);

            IProductSearchSpecification brandSpecification =
                new ProductIsInBrandSpecification(request.BrandIds);

            IProductSearchSpecification sizeSpecification =
                new ProductIsInSizeSpecification(request.SizeIds);

            IProductSearchSpecification categorySpecification =
                new ProductIsInCategorySpecification(request.CategoryId);

            IEnumerable<Product> matchingProducts = FindAllProducts().Where(colourSpecification.IsSatisfiedBy)
                .Where(brandSpecification.IsSatisfiedBy)
                .Where(sizeSpecification.IsSatisfiedBy)
                .Where(categorySpecification.IsSatisfiedBy);

            switch (request.SortBy)
            {
                case ProductsSortBy.PriceLowToHigh:
                    matchingProducts = matchingProducts.OrderBy(p => p.Price);
                    break;
                case ProductsSortBy.PriceHighToLow:
                    matchingProducts = matchingProducts.OrderByDescending(p => p.Price);
                    break;
            }

            GetProductsByCategoryResponse response = matchingProducts.CreateProductSearchResultFrom(request);

            response.SelectedCategoryName =
                GetAllCategories().Categories.Where(c => c.Id == request.CategoryId).FirstOrDefault().Name;                                

            return response;
        }
        
        public GetProductResponse GetProduct(GetProductRequest request)
        {
            GetProductResponse response = new GetProductResponse();

            response.Product = FindAllProductTitles().Where(p => p.Id == request.ProductId).FirstOrDefault().ConvertToProductDetailView();

            return response;
        }

        public GetAllCategoriesResponse GetAllCategories()
        {
            lock (_getAllCategoriesLock)
            {
                GetAllCategoriesResponse response =
                    _cachStorage.Retrieve<GetAllCategoriesResponse>(CacheKeys.AllCategories.ToString());

                if (response == null)
                {
                    response = _realProductCatalogService.GetAllCategories();
                    _cachStorage.Store(CacheKeys.AllCategories.ToString(), response);
                }

                return response;
            }
        }
    }
}
    