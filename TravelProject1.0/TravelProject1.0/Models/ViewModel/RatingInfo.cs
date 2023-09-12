using TravelProject1._0.Models.DTO;

namespace TravelProject1._0.Models.ViewModel
{
    public class RatingInfo
    {
        public int PlanId { get; set; }

        public decimal? UnitPrice { get; set; }

        public short? Quantity { get; set; }

        public DateTime? UseDate { get; set; }

        public string? Odimg { get; set; }

        public string? Odname { get; set; }
    }
}
