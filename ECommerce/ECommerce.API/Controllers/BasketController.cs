﻿using System;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using UserActor.Interfaces;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    public class BasketController : Controller
    {
        private IUserActor GetActor(string userId)
        {
            return ActorProxy.Create<IUserActor>(new ActorId(userId), new Uri("fabric:/ECommerce/UserActorService"));
        }

        [HttpGet("{userId}")]
        public async Task<ApiBasket> GetBasketAsync(string userId)
        {
            var actor = GetActor(userId);

            var products = await actor.GetBasketAsync();

            return new ApiBasket
            {
                UserId = userId,
                Items = products.Select(p => new ApiBasketItem { ProductId = p.Key.ToString(), Quantity = p.Value }).ToArray()
            };
        }

        [HttpPost("{userId}")]
        public Task AddToBasketAsync(string userId, [FromBody] ApiBasketAddRequest request)
        {
            var actor = GetActor(userId);

            return actor.AddToBasketAsync(request.ProductId, request.Quantity);
        }

        [HttpDelete("{userId}")]
        public Task ClearBasketAsync(string userId)
        {
            var actor = GetActor(userId);

            return actor.ClearBasketAsync();
        }
    }
}
