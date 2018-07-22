﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ECommerce.API.Models;

using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<ApiProduct>> GetProductsAsync()
        {
            return new[] { new ApiProduct() { Id = Guid.NewGuid(), Description = "fake"} };
        }

        // GET api/values/5
        [HttpPost("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
