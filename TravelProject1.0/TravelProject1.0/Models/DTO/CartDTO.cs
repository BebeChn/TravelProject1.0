namespace TravelProject1._0.Models.DTO
{
    public class CartDTO
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public string? Img { get; set; }

        public int PlanId { get; set; }

        public string Name { get; set; } = null!;

        public int Id { get; set; }

        public DateTime? Date { get; set; }

        public decimal Price { get; set; }
    }
}
