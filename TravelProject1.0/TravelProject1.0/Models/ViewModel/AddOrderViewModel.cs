using TravelProject1._0.Models.DTO;

namespace TravelProject1._0.Models.ViewModel
{
    public class AddOrderViewModel
    {
        public int UserId { get; set; }

        public string? OrderName { get; set; }

        public int PlanId { get; set; }

        public decimal? UnitPrice { get; set; }

        public short? Quantity { get; set; }
    }
}
