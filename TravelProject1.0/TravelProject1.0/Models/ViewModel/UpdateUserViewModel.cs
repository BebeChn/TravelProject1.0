using TravelProject1._0.Models.DTO;
namespace TravelProject1._0.Models.ViewModel
{
    public class UpdateUserViewModel : PostUserVewModel
    {
        public int UserId { get; set; }

        public string Password { get; set; }

        public string Name { get; set; } 

        public string Email { get; set; }

        public DateTime? Birthday { get; set; }


        public string? Gender { get; set; }

        public string Phone { get; set; }

        public string PasswordHash { get; set; } 
        public string Salt { get; set; }

    }
}