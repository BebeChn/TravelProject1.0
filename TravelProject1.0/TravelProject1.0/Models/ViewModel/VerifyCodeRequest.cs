namespace TravelProject1._0.Models.ViewModel
{
    public class VerifyCodeRequest
    {
            public string CodeId { get; set; }
            public string Code { get; set; }

        public DateTime ExpiryTime { get; set; }

    }
}
