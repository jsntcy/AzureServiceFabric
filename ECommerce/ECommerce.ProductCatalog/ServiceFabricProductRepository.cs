﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ECommerce.ProductCatalog.Model;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace ECommerce.ProductCatalog
{
    class ServiceFabricProductRepository : IProductRepository
    {
        private readonly IReliableStateManager _stateManager;

        public ServiceFabricProductRepository(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var products = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Product>>("products");
            var result = new List<Product>();

            using (var tx = _stateManager.CreateTransaction())
            {
                var allProducts = await products.CreateEnumerableAsync(tx, EnumerationMode.Unordered);

                using (var enumerator = allProducts.GetAsyncEnumerator())
                {
                    while (await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        KeyValuePair<Guid, Product> current = enumerator.Current;
                        result.Add(current.Value);
                    }
                }
            }

            return result;
        }

        public async Task AddProductAsync(Product product)
        {
            var products = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Product>>("products");

            using (var tx = _stateManager.CreateTransaction())
            {
                await products.AddOrUpdateAsync(tx, product.Id, product, (id, value) => product);
                await tx.CommitAsync();
            }
        }

        public async Task<Product> GetProductAsync(Guid productId)
        {
            var products = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Product>>("products");

            using (var tx = _stateManager.CreateTransaction())
            {
                var product = await products.TryGetValueAsync(tx, productId);

                return product.HasValue ? product.Value : null;
            }
        }
    }
}
