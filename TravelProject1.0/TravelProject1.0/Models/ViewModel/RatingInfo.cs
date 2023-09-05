using TravelProject1._0.Models.DTO;

namespace TravelProject1._0.Models.ViewModel
{
    public class RatingInfo
    {
        public int PlanId { get; set; }

        public int ProductId { get; set; }

        public string Name { get; set; } = null!;

        public string? PlanImg { get; set; }

        public IEnumerable<OrderDetailDto> OrderDetails { get; set; }
    }
}
