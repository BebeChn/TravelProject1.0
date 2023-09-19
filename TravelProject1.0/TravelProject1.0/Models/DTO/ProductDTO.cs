
namespace TravelProject1._0.Models.ProductDTO
{
    public class ProductDTO
    {
        public int ProductId { get; set; }

        public int Id { get; set; }

        public string ProductName { get; set; } = null!;

        public decimal Price { get; set; }

        public string? Img { get; set; }
    }
}
