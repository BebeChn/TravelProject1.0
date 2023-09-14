using System.ComponentModel.DataAnnotations;

namespace TravelProject1._0.Models.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int PlanId { get; set; }

        public short? Quantity { get; set; }

        public decimal? UnitPrice { get; set; }

        public DateTime? UseDate { get; set; }

        public float? Discount { get; set; }
    }
}
