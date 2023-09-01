using System.ComponentModel.DataAnnotations;

namespace TravelProject1._0.Models.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage ="電子郵件為必填")]
        [EmailAddress]
        public string Email { get; set; }

        public DateTime? Birthday { get; set; }

        [Required(ErrorMessage = "密碼為必填")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Required]
        public string? Gender { get; set; }
        [Required]
        public string Phone { get; set; } = null!;

        [Display(Name="記住我?")]
        public bool RememberMe { get; set; }

        public string PasswordHash { get; set; } = null!;

        public string Salt { get; set; } = null!;

        public string VerificationCode { get; set; } = null!;
        
    }
}
