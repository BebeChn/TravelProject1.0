namespace TravelProject1._0.Models.ViewModel
{
    public class ResetPasswordViewModel:PostUserVewModel
    {
        public string Email { get; set; }
        public string ResetToken { get; set; }

        public string NewPassword { get; set; }
    }
}