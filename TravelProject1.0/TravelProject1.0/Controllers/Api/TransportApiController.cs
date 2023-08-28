﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TransportApiController : ControllerBase
    {
        private readonly TravelProjectAzureContext _dbContext;

        public TransportApiController(TravelProjectAzureContext dbContext)
        {
            _dbContext = dbContext;
        }

        //取得交通種類的商品
        [HttpGet]
        public async Task<IEnumerable<TransportDTO>> GetCategoryByTransport()
        {
            return _dbContext.Products.Where(p => p.Id == 3).Select(p => new TransportDTO
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Price = p.Price,
                MainDescribe = p.MainDescribe,
                Img = p.Img
            });
        }

        //排序商品的價格
        //低到高
        [HttpGet]
        public async Task<IEnumerable<TransportDTO>> TransportOrderbyPrice()
        {
            return _dbContext.Products.Where(p => p.Id == 3).OrderBy(p => p.Price).Select(p => new TransportDTO
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Price = p.Price,
                MainDescribe = p.MainDescribe,
                Img = p.Img
            });
        }
        //高到低
        [HttpGet]
        public async Task<IEnumerable<TransportDTO>> TransportOrderByDescendingPrice()
        {
            return _dbContext.Products.Where(p => p.Id == 3).OrderByDescending(p => p.Price).Select(p => new TransportDTO
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Price = p.Price,
                MainDescribe = p.MainDescribe,
                Img = p.Img
            });
        }
    }
}
