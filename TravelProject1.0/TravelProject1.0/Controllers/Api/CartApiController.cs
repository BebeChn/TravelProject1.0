using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelProject1._0.Helper;
using TravelProject1._0.Models;
using TravelProject1._0.Models.ViewModel;

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/CartApi/[action]")]
    [ApiController]
    public class CartApiController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TravelProjectAzureContext _context;
        public CartApiController(ILogger<HomeController> logger, TravelProjectAzureContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            return null;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] CartViewModel model)
        {
            if (model == null) return BadRequest();

            Cart item = new Cart
            {
                UserId = model.UserId,
                ProductId = model.ProductId,
                CartName = model.CartName,
                CartPrice = model.CartPrice,
                CartQuantity = model.CartQuantity,
                CartDate = model.CartDate
            };

            _context.Carts.Add(item);

            try
            {
                _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok(new { Message = "商品已加入購物車" });
        }
    }
}
