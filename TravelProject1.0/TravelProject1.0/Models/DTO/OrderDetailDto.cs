using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelProject1._0.Models.DTO
{
    public class OrderDetailDto
    {
        public int PlanId { get; set; }

        public short? Quantity { get; set; }

        public decimal? UnitPrice { get; set; }

<<<<<<< HEAD
        public DateTime? UseDate { get; set; }
=======
        public string? Odname { get; set; }
        public string? Odimg { get; set; }
>>>>>>> Bebe
    }
}
