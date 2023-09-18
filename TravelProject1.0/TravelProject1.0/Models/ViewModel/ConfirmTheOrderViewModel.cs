using TravelProject1._0.Models.DTO;

namespace TravelProject1._0.Models.ViewModel
{
    public class ConfirmTheOrderViewModel
    {
        public int OrderId { get; set; }

        public int UserId { get; set; }

        public DateTime? OrderDate { get; set; }

        public string? Odname { get; set; }

        public short? Quantity { get; set; }

        public decimal? UnitPrice { get; set; }

        public IEnumerable<ConfirmTheOrderDetailDto> ConfirmTheOrderDetails { get; set; }
    }
}
