using System.ComponentModel.DataAnnotations;

namespace TravelProject1._0.Models.ViewModel
{
    public class VerificationCodeViewModel
    {
              [Key]
            public int CodeId { get; set; }
            public string? Code { get; set; }

        //public DateTime ExpiryTime { get; set; }

    }
}
