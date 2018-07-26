using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ECommerce.API.Models;
using ECommerce.ProductCatalog.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductCatalogService _productCatalogService;

        public ProductsController()
        {
            _productCatalogService = ServiceProxy.Create<IProductCatalogService>(
                new Uri("fabric:/ECommerce/ProductCatalog"),
                new ServicePartitionKey(0));
        }

        [HttpGet]
        public async Task<IEnumerable<ApiProduct>> GetAllProductsAsync()
        {
            var allProducts = await _productCatalogService.GetAllProductsAsync();

            return allProducts.Select(product => new ApiProduct
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                IsAvailable = product.Availability > 0
            });
        }

        [HttpPost]
        public Task AddProductAsync([FromBody]ApiProduct product)
        {
            var newProduct = new Product
            {
                Id = Guid.NewGuid(),
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Availability = 100
            };

            return _productCatalogService.AddProductAsync(newProduct);
        }
    }
}
