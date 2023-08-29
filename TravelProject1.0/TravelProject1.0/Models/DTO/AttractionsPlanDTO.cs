namespace TravelProject1._0.Models.DTO
{
    public class AttractionsPlanDTO
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public string? MainDescribe { get; set; }

        public string? SubDescribe { get; set; }

        public string? ShortDescribe { get; set; }

        public string? Img { get; set; }

        public int PlanId { get; set; }

        public string Name { get; set; } = null!;

        public string? Describe { get; set; }

        public string? PlanImg { get; set; }
    }
}
