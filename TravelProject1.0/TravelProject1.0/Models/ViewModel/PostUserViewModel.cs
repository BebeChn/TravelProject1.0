using System.ComponentModel.DataAnnotations;

namespace TravelProject1._0.Models.ViewModel
{
    public class PostUserVewModel
    {

        public string Name { get; set; } = null!;

        public string Email { get; set; }

        public DateTime? Birthday { get; set; }

        public string Password { get; set; }
      
        public string? Gender { get; set; }

        public string Phone { get; set; } = null!;

        public string? Address { get; set; }

    }
}

