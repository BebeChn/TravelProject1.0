namespace TravelProject1._0.Models.DTO
{
    public class ConfirmTheOrderDTO
    {
        public int UserId { get; set; }

        public string Name { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Email { get; set; } = null!;

        public int? Points { get; set; }
    }
}
