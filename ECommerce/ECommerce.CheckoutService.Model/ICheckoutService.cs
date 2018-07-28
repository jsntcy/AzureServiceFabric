using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace ECommerce.CheckoutService.Model
{
    public interface ICheckoutService : IService
    {
        Task<CheckoutSummary> CheckoutAsync(string userId);

        Task<IEnumerable<CheckoutSummary>> GetOrderHitoryAsync(string userId);
    }
}
