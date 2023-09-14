namespace TravelProject1._0.Models.DTO
{
    public class SuperFilterDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Account { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Role { get; set; } = null!;

        public string Describe { get; set; } = null!;

        public string? CreateDate { get; set; }

        public string? LoginDate { get; set; }

        public int? RoleId { get; set; }
    }
}
