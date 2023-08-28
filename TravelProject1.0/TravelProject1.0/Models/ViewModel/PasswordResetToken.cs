namespace TravelProject1._0.Models.ViewModel
{
    internal class PasswordResetToken
    {

        public int Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationTime { get; set; }

    }
}