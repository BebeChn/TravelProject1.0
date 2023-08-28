namespace TravelProject1._0.Models.ViewModel
{
    public class ResetPasswordViewModel:PostUserVewModel
    {
        public string Email { get; set; }
        public string Salt { get; set; }

        public string NewPassword { get; set; }
        public string PasswordHash { get; set; }
    }
}