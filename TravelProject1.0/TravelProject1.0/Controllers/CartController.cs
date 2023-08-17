using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelProject1._0.Helper;
using TravelProject1._0.Models;

namespace TravelProject1._0.Controllers
{
    public class CartController : Controller
    {
        public TravelUsersContext _context;

        public IActionResult Index()
        {
            //向 Session 取得商品列表
            List<CartItem> CartItems = Session.
                GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");

            //計算商品總額
            if (CartItems != null)
            {
                ViewBag.Total = CartItems.Sum(m => m.SubTotal);
            }
            else
            {
                ViewBag.Total = 0;
            }

            return View(CartItems);
        }
        public IActionResult AddtoCart(int id)
        {
            //取得商品資料
            CartItem item = new CartItem
            {
                Product = _context.Products.Single(x => x.ProductId.Equals(id)),
    
                Amount = 1,
                SubTotal = (int)_context.Products.Single(m => m.ProductId == id).Price
            };

            //判斷 Session 內有無購物車
            if (Session.
                GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart") == null)
            {
                //如果沒有已存在購物車: 建立新的購物車
                List<CartItem> cart = new List<CartItem>();
                cart.Add(item);
                Session.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                //如果已存在購物車: 檢查有無相同的商品，有的話只調整數量
                List<CartItem> cart = Session.
                    GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");

                int index = cart.FindIndex(m => m.Product.ProductId.Equals(id));
                if (index != -1)
                {
                    cart[index].Amount += item.Amount;
                    cart[index].SubTotal += item.SubTotal;
                }
                else
                {
                    cart.Add(item);
                }
                Session.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }

            return NoContent(); 
        }
    }
}
