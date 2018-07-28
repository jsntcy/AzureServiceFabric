using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.API.Models;
using ECommerce.CheckoutService.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    public class CheckoutController : Controller
    {
        private static readonly Random rnd = new Random(DateTime.UtcNow.Second);

        [Route("{userId}")]
        public async Task<ApiCheckoutSummary> CheckoutAsync(string userId)
        {
            var checkoutSummary = await GetCheckoutService().CheckoutAsync(userId);

            return ToApiCheckoutSummary(checkoutSummary);
        }

        [Route("history/{userId}")]
        public async Task<IEnumerable<ApiCheckoutSummary>> GetOrderHistoryAsync(string userId)
        {
            IEnumerable<CheckoutSummary> history = await GetCheckoutService().GetOrderHitoryAsync(userId);

            return history.Select(ToApiCheckoutSummary);
        }

        private ApiCheckoutSummary ToApiCheckoutSummary(CheckoutSummary checkoutSummary)
        {
            return new ApiCheckoutSummary
            {
                Products = checkoutSummary.Products.Select(p => new ApiCheckoutProduct
                {
                    ProductId = p.Product.Id,
                    ProductName = p.Product.Name,
                    Price = p.Price,
                    Quantity = p.Quantity
                }).ToList(),
                Date = checkoutSummary.Date,
                TotalPrice = checkoutSummary.TotalPrice
            };
        }

        private ICheckoutService GetCheckoutService()
        {
            long key = LongRandom();

            return ServiceProxy.Create<ICheckoutService>(
                   new Uri("fabric:/ECommerce/CheckoutService"),
                   new ServicePartitionKey(key));
        }

        private long LongRandom()
        {
            byte[] buf = new byte[8];
            rnd.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);
            return longRand;
        }
    }
}
