using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace ECommerce.ProductCatalog.Model
{
    public interface IProductCatalogService : IService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task AddProductAsync(Product product);
    }
}
