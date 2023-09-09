using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TravelProject1._0.Helper;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;
using TravelProject1._0.Models.ViewModel;
using TravelProject1._0.Services;

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartApiController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TravelProjectAzureContext _context;
        private readonly IUserIdentityService _userIdentityService;
        public CartApiController(ILogger<HomeController> logger, TravelProjectAzureContext context,IUserIdentityService userIdentityService)
        {
            _logger = logger;
            _context = context;
            _userIdentityService = userIdentityService;
        }

        //取得購物車商品
        [HttpGet]   
        [Authorize]
        public async Task<IQueryable<CartViewModel>> GetCart()
        {
            //Claim user = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            //string? idu = user.Value;
            //int id = Convert.ToInt32(idu);
            int userId=_userIdentityService.GetUserId();

            return _context.Carts.Where(c => c.UserId == userId).Select(c => new CartViewModel
            {
                PlanId = c.PlanId,
                CartName = c.CartName,
                CartPrice = c.CartPrice,
                CartQuantity = c.CartQuantity,
                CartDate = c.CartDate.GetValueOrDefault().ToString("u")
            });
        }

        //加入購物車
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddCartViewModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
       
            int userId = _userIdentityService.GetUserId();
            Cart item = new Cart
            {
                UserId = userId,
                PlanId = model.PlanId,
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

        //刪除購物車項目
        [HttpDelete]
        public async Task<IActionResult> RemoveFromCart([FromBody] CartViewModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
               var userId = _userIdentityService.GetUserId();

            var cartItem = await _context.Carts.FirstOrDefaultAsync(c =>
                c.UserId == userId && c.PlanId == model.PlanId);

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

        //購物車項目移至訂單
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] List<AddOrderViewModel> models)
        {
            if (models == null || models.Count == 0) return BadRequest();

            int userId = _userIdentityService.GetUserId();

            try
            {
                Order order = new Order
                {
                    UserId = userId,

                    OrderDate = DateTime.Now
                };

                _context.Add(order);
                await _context.SaveChangesAsync();

                foreach (var model in models)
                {
                    OrderDetail orderDetail = new OrderDetail
                    {
                        OrderId = order.OrderId,
                        PlanId = model.PlanId,
                        Odname = model.Odname,
                        Quantity = model.Quantity,
                        UnitPrice = model.UnitPrice
                    };

                    _context.Add(orderDetail);
                    await _context.SaveChangesAsync();
                }

                Response.Headers.Add("OrderID", order.OrderId.ToString());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok();
        }
    }
}