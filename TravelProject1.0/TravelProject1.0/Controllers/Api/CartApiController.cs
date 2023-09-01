using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
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
        public CartApiController(ILogger<HomeController> logger, TravelProjectAzureContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IQueryable<CartViewModel>> GetCart(int id)
        {
            return _context.Carts.Where(c => c.UserId == id).Select(c => new CartViewModel
            {
                CartName = c.CartName,
                CartPrice = c.CartPrice,
                CartQuantity = c.CartQuantity,
                CartDate = c.CartDate
            });
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
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok(new { Message = "商品已加入購物車" });
        }
        public async Task<IActionResult> RemoveFromCart([FromBody] CartViewModel model)
        {
            if (model == null) return BadRequest();

            var cartItem = await _context.Carts.FirstOrDefaultAsync(c =>
                c.UserId == model.UserId && c.ProductId == model.ProductId);

            if (cartItem == null)
            {
                return NotFound(new { Message = "找不到要移除的商品" });
            }

            _context.Carts.Remove(cartItem);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok(new { Message = "商品已從購物車移除" });
        }
    }
}
