using System.ComponentModel;

namespace TravelProject1._0.ViewModel
{
    public class AdminViewModel
    {
        [DisplayName("xxxxx")]
        public int UserId { get; set; }

        public string Name { get; set; } = null!;

        public string? Gender { get; set; }

        public string? Address { get; set; }
    }
}
