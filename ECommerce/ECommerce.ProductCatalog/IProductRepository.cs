using System.Collections.Generic;
using System.Threading.Tasks;

using ECommerce.ProductCatalog.Model;

namespace ECommerce.ProductCatalog
{
    interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();

        Task AddProductAsync(Product product);
    }
}
