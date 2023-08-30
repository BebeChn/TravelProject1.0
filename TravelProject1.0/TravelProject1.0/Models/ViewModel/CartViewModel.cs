namespace TravelProject1._0.Models.ViewModel
{
    public class CartViewModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }
        public short? Quantity { get; set; }
        public short? UnitPrice { get; set;}
    }
}
