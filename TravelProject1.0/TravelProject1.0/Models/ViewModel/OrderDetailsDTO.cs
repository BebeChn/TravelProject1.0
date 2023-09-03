using TravelProject1._0.Models.DTO;

namespace TravelProject1._0.Models.ViewModel
{
    public class OrderInfo
    {
        public DateTime? OrderDate { get; internal set; }
        public int OrderId { get; internal set; }
        public IEnumerable<OrderDetailDto> Detail { get; set; }
    }
}
