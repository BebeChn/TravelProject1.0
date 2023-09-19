namespace TravelProject1._0.Models.DTO
{
    public class PaymentDto
    {
        public IEnumerable<PaymentDetailDto> detailDTOs { get; set; }

        public int Points { get; set; }

        public int TotalPrice { get; set; }
    }
    public class PaymentDetailDto
    {
        public int PlanId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public string PlanName { get; set; }
    }
}
