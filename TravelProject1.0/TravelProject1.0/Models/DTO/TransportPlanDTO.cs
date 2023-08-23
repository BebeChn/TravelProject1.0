namespace TravelProject1._0.Models.DTO
{
    public class TransportPlanDTO
    {
        public string ProductName { get; set; } = null!;

        public string? MainDescribe { get; set; }

        public string? SubDescribe { get; set; }

        public string? ShortDescribe { get; set; }

        public int PlanId { get; set; }

        public int ProductId { get; set; }

        public string Name { get; set; } = null!;

        public string? Describe { get; set; }
    }
}
