using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using TravelProject1._0.Models;
using TravelProject1._0.Models.ViewModel;
using TravelProject1._0.Services;

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/Cart/[action]")]
    [ApiController]
    public class CartApiController : ControllerBase
    {
        private readonly TravelProjectAzureContext _context;
        private readonly IUserIdentityService _userIdentityService;
        public CartApiController(TravelProjectAzureContext context, IUserIdentityService userIdentityService)
        {
            _context = context;
            _userIdentityService = userIdentityService;
        }

        //取得購物車商品
        [HttpGet]
        [Authorize]
        public async Task<List<CartViewModel>> GetCart()
        {
            int userId = _userIdentityService.GetUserId();

            return await _context.Carts.Where(c => c.UserId == userId).Select(c => new CartViewModel
            {
                PlanId = c.PlanId,
                CartName = c.CartName,
                CartPrice = c.CartPrice,
                CartQuantity = c.CartQuantity,
                CartDate = c.CartDate.GetValueOrDefault().ToString("u")
            }).ToListAsync();
        }

        //加入購物車
        [HttpPost]
        public async Task<bool> AddToCart([FromBody] AddCartViewModel model)
        {
            if (model == null) return false;
            int userId = _userIdentityService.GetUserId();

            try
            {
                _context.Carts.Add(new Cart
                {
                    UserId = userId,
                    PlanId = model.PlanId,
                    CartName = model.CartName,
                    CartPrice = model.CartPrice,
                    CartQuantity = model.CartQuantity,
                    CartDate = model.CartDate
                });
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //刪除購物車項目
        [HttpDelete]
        public async Task<bool> RemoveFromCart([FromBody] CartViewModel model)
        {
            if (model == null) return false;
            var userId = _userIdentityService.GetUserId();
            try
            {
                var cartItem = await _context.Carts.FirstOrDefaultAsync(c =>
                c.UserId == userId && c.PlanId == model.PlanId);
                if (cartItem == null) return false;

                _context.Carts.Remove(cartItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}