namespace TravelProject1._0.Models.DTO
{
    public class PaymentDTO
    {
        public IEnumerable<PaymentDetailDTO> detailDTOs { get; set; }
        public int Points { get; set; }
    }
    public class PaymentDetailDTO
    {
        public int PlanId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string PlanName { get; set; }
    }
}
