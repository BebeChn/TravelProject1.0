namespace TravelProject1._0.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }  
        public int Amount { get; set; }     
        public int SubTotal { get; set; } 
    }

    public class CartItem : OrderItem
    {
        public Product Product { get; set; } 
        public string imageSrc { get; set; }
    }
}
