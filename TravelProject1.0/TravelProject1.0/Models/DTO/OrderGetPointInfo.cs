namespace TravelProject1._0.Models.DTO
{
    public class OrderGetPointDto
    {
        public int? TotalPrice { get; set; }

        public int? NewPoint { get; set; }

        public DateTime? OrderDate { get; internal set; }

        public int OrderId { get; internal set; }
    }
}
