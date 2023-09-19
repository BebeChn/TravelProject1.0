namespace TravelProject1._0.Areas.Admin.Models.DTO
{
    public class SearchOrdersDTO
    {
        public int OrderId { get; set; }

        public int UserId { get; set; }

        public int? TotalPrice { get; set; }

        public int? NewPoint { get; set; }

        public DateTime? OrderDate { get; set; }

        public string? Status { get; set; }
    }
}
