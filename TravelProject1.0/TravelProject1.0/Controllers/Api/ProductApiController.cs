﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using TravelProject1._0.Models;
using TravelProject1._0.Models.ProductDTO;

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly TravelProjectAzureContext _db;

        public ProductApiController(TravelProjectAzureContext db)

        {
            _db = db;
        }
        public async Task<List<ProductDTO>> GetImge(int id)
        {
            var Product = _db.Products.Select(x =>
            new ProductDTO
            {
                ProductName = x.ProductName,
                Price = x.Price,
                ProductId = x.Id,
                Image = "/lib/image/foreast.jpeg"
            }).ToList();
            return Product;
        }
    }
}