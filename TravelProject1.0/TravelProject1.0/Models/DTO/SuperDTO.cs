
namespace TravelProject1._0.Models.DTO
{
	public class SuperDTO
	{
		public int Id { get; set; }

		public string Name { get; set; } = null!;
		public string Account { get; set; } = null!;

		public string Password { get; set; } = null!;

		public string Role { get; set; } = null!;
		public string Describe { get; set; } = null!;

		public DateTime? CreateDate { get; set; }

		public DateTime? LoginDate { get; set; }

		public int? RoleId { get; set; }
	}
}
