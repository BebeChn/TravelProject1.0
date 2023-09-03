namespace TravelProject1._0.Models.ViewModel
{
    public class AddCartViewModel
    {
        public int UserId { get; set; }

        public int ProductId { get; set; }

        public short? CartQuantity { get; set; }

        public decimal? CartPrice { get; set; }

        public string? CartName { get; set; }

        public DateTime? CartDate { get; set; }
    }
}
