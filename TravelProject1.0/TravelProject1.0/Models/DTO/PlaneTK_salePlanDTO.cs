namespace TravelProject1._0.Models.DTO
{
    public class PlaneTK_salePlanDTO
    {


        public int PlanId { get; set; }

        public int ProductId { get; set; }

        public string Name { get; set; } = null!;

        public string? Describe { get; set; }

        public string? PlanImg { get; set; }

        public decimal PlanPrice { get; set; }
    }
}
