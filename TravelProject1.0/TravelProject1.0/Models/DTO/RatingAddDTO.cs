namespace TravelProject1._0.Models.DTO
{
    public class RatingAddDTO
    {
        public int ProductId { get; set; }

        public short RatingScore { get; set; }

        public string? Describe { get; set; }

        public DateTime RatingDate { get; set; }
    }
}
