using System.ComponentModel.DataAnnotations;

namespace TravelProject1._0.Models.DTO
{
    public class UserDTO
    {

        public string Name { get; set; } = null!;

        [Required(ErrorMessage ="電子郵件為必填")]
        [EmailAddress]
        public string Email { get; set; }


        [Required(ErrorMessage = "密碼為必填")]
        public string Password { get; set; } = null!;

        public string? Gender { get; set; }

        public string Phone { get; set; } = null!;

        public bool 記住我 { get; set; } 
    }
}
