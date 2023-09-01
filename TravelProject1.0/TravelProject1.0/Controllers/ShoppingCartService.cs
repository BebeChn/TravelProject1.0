using TravelProject1._0.Models.ViewModel;

namespace TravelProject1._0.Controllers
{
    public class ShoppingCartService
    {
        private readonly List<ShoppingCartItemViewModel> _cartItems = new List<ShoppingCartItemViewModel>();

        public void AddToCart(ShoppingCartItemViewModel item)
        {
            _cartItems.Add(item);
        }

        public void RemoveFromCart(int itemId)
        {
            var itemToRemove = _cartItems.FirstOrDefault(item => item.Id == itemId);
            if (itemToRemove != null)
            {
                _cartItems.Remove(itemToRemove);
            }
        }

        public List<ShoppingCartItemViewModel> GetCartItems()
        {
            return _cartItems;
        }
    
}
}
