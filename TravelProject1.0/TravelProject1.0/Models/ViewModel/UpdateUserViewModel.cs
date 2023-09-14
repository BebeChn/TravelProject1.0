using System.ComponentModel.DataAnnotations;
using TravelProject1._0.Models.DTO;
namespace TravelProject1._0.Models.ViewModel
{
    public class UpdateUserViewModel
    {
        public string Name { get; set; }

        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }

        public string? Gender { get; set; }

        public string Phone { get; set; }
    }
}