using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelProject1._0.Helper;
using TravelProject1._0.Models;
using TravelProject1._0.Models.ViewModel;

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/[controller]")]
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
        public string AddtoCart(int id)
        {
            Product product = _context.Products.SingleOrDefault(p => p.Id == id);

            if (product == null)
            {
                return "沒有這個商品";
            }

            var cartItem = new CartViewModel
            {
                ProductId = product.Id,
                Product = product,
                Quantity = 1
            };
            List<CartViewModel> cart = HttpContext.Session.GetObjectFromJson<List<CartViewModel>>("cart");
            if (cart == null)
            {
                cart = new List<CartViewModel>();
            }
            var existingCartItem = cart.FirstOrDefault(item => item.ProductId == cartItem.ProductId);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity += 1;
            }
            else
            {
                cart.Add(cartItem);
            }

            HttpContext.Session.SetObjectAsJson("cart", cart);
            return "商品已成功添加到購物車";
        }
        public IActionResult RemoveItem(int id)
        {
            //向 Session 取得商品列表
            List<CartViewModel> cart = Session.
                GetObjectFromJson<List<CartViewModel>>(HttpContext.Session, "cart");

            //用FindIndex查詢目標在List裡的位置
            int index = cart.FindIndex(m => m.Product.Id.Equals(id));
            cart.RemoveAt(index);

            if (cart.Count < 1)
            {
                Session.Remove(HttpContext.Session, "cart");
            }
            else
            {
                Session.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }

            return Ok(cart);
        }

    }
}
