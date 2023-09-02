namespace TravelProject1._0.Models.ViewModel
{
    public class OrderDetailsDTO
    {
       

       public int UserId { get; set; }
        public int OrderId { get; set; }

        public int PlanId { get; set; }

        public decimal? UnitPrice { get; set; }

        public short? Quantity { get; set; }

      
    }
}
