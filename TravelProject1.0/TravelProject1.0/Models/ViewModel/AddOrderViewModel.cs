using TravelProject1._0.Models.DTO;

namespace TravelProject1._0.Models.ViewModel
{
    public class AddOrderViewModel
    {
        public int UserId { get; set; }

        public string? OrderName { get; set; }

        public IEnumerable<AddOrder> orderDetails { get; set; }
    }
}
