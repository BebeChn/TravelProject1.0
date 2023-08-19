using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelProject1._0.Models
{
    [Table("Orders")]
    public class Order
    {
        public Order() { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        [ForeignKey(nameof(IdentityUser))]
        [Column(TypeName = "nvarchar")]
        [MaxLength(450)]
        public string Id { get; set; }

        public DateTime OrderDate { get; set; }

     

        public virtual IdentityUser IdentityUser { get; set; }
    }
}