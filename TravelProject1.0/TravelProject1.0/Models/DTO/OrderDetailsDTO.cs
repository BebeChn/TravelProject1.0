namespace TravelProject1._0.Models.DTO
{
    public class OrderInfo
    {
        public int OrderId { get; set; }

        public int PlanId { get; set; }

        public decimal? UnitPrice { get; set; }

        public short? Quantity { get; set; }

        public DateTime? UseDate { get; set; }

        public string? Odimg { get; set; }

        public string? Odname { get; set; }

        public int ProductId { get; set; }
    }
}
