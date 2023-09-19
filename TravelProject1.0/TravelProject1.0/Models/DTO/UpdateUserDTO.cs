using System.ComponentModel.DataAnnotations;

namespace TravelProject1._0.Models.DTO
{
    public class UpdateUserDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string Birthday { get; set; }

        public string? Gender { get; set; }

        public string? Phone { get; set; }
    }
}
