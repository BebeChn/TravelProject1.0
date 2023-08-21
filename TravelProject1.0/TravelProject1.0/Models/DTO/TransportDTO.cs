namespace TravelProject1._0.Models.DTO
{
    public class TransportDTO
    {
        public int ProductId { get; set; }

        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public string? MainDescribe { get; set; }
    }
}
