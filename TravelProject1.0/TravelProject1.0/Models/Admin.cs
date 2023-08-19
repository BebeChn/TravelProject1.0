using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelProject1._0.Models
{
    [Table("Admin")]
    public class Admin
    {
        public Admin() { }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdminId { get; set; }
        
        [Column(TypeName = "nvarchar")]
        [MaxLength(30)]
        public string AdminName { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Account { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Password { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(10)]
        public string Role { get; set; }
        [Column(TypeName = "nvarchar")]
        [MaxLength(2000)]
        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LoginDate { get; set; }
    }
}
