using TravelProject1._0.Models.ViewModel;

namespace TravelProject1._0.Controllers.Api
{
    public class ForgotPasswordViewModel:PostUserVewModel
    {
        public string Email { get; set; }

        public string? VerificationCode { get; set; }
    }

}