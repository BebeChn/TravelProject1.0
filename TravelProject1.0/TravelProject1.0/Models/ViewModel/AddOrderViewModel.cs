using TravelProject1._0.Models.DTO;

namespace TravelProject1._0.Models.ViewModel
{
    public class AddOrderViewModel
    {
        public int OrderId { get; set; }

        public int UserId { get; set; }

        public DateTime? OrderDate { get; set; }

        public string? OrderName { get; set; }

        public IEnumerable<OrderDetailDto> orderDetails { get; set; }
    }
}
