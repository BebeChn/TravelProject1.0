using System.ComponentModel;

namespace TravelProject1._0.Models.DTO
{
    public class AdminDTO
    {
        [DisplayName("ID")]
        public int UserId { get; set; }
        [DisplayName("名字")]
        public string Name { get; set; } = null!;
        [DisplayName("性別")]
        public string? Gender { get; set; }
        [DisplayName("地址")]
        public string? Address { get; set; }
        [DisplayName("信箱")]
        public string? Email { get; set; }
    }
}
