using System.ComponentModel.DataAnnotations;

namespace TravelProject1._0.Areas.Admin.Models.ViewModel
{
    public class AdminGetUserViewModel
    {
        public string? Birthday { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public string? Gender { get; set; }

        public string Phone { get; set; }
    }
}
