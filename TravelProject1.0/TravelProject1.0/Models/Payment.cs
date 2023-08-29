namespace TravelProject1._0.Models
{
    public class Payment
    {
        public string MerchantID { get; set; }

        public string HashKey { get; set;}

        public string HashIV { get; set;}

        public string Version { get; set;}

        public string ReturnURL { get; set;}

        public string NotifyURL { get; set; }

        public string ClientBackURL { get; set; }
    }
}
