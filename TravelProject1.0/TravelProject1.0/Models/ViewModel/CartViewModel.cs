namespace TravelProject1._0.Models.ViewModel
{
    public class CartViewModel
    {
        public int UserId { get; set; }

        public int PlanId { get; set; }

        public short? CartQuantity { get; set; }

        public decimal? CartPrice { get; set; }

        public string? CartName { get; set; }

        public string CartDate { get; set; }
    }
}
