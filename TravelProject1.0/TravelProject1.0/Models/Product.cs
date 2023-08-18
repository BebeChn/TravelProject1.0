using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace TravelProject1._0.Models
{
    [Table("Product")]
    public class Product
    {
        public Product() { }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string ProductName { get; set; }


        public decimal Price { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(20000)]
        public string? ProductDescription { get; set; }
       
        [Column(TypeName = "nvarchar")]
        [MaxLength(20000)]
        public string? ProductDescription2 { get; set; }
       
        [Column(TypeName = "nvarchar")]
        [MaxLength(20000)]
        public string? ProductDescription3 { get; set; }
    }
}