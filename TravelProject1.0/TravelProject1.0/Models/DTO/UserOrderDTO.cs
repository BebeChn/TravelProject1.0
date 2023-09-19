namespace TravelProject1._0.Models.DTO
{
    public class UserOrderDto
    {
        public int OrderId { get; set; }

        public int UserId { get; set; }

        public string OrderDate { get; set; }

        public string? Status { get; set; }
    }
}
