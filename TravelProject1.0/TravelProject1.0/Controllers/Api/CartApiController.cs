using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelProject1._0.Helper;
using TravelProject1._0.Models;
using TravelProject1._0.Models.ViewModel;

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartApiController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TravelProjectAzureContext _context;

        private readonly ShoppingCartService _cartService;
        public CartApiController(ILogger<HomeController> logger, TravelProjectAzureContext context, ShoppingCartService cartService)
        {
            _logger = logger;
            _context = context;
            _cartService = cartService;
        }  
        [HttpPost]
        public IActionResult AddToCart(int productId, string productName, decimal price, int quantity)
        {
            var item = new ShoppingCartItemViewModel
            {
                Id = productId,
                ProductName = productName,
                Price = price,
                Quantity = quantity
            };

            _cartService.AddToCart(item);

            return Ok(new { Message = "商品已加入購物車" });
        }

        
    }


}
