using System.Drawing;

namespace TravelProject1._0.Models.ProductDTO
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }

        public string Image { get; set; }
    }
}
