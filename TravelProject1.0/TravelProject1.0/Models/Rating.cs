using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelProject1._0.Models
{
    [Table("Rating")]
    public class Rating
    {
        public Rating() { }
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RatingId { get; set; }

        [ForeignKey(nameof(IdentityUser))]
        [Column(TypeName = "nvarchar")]
        [MaxLength(450)]
        public string Id { get; set; }
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set;}

        public string RatingScore { get; set;}
        [Column(TypeName = "nvarchar")]
        [MaxLength(200)]
        public string Describe { get; set; }

        public virtual IdentityUser IdentityUser { get; set; }
    }
}
