using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelProject1._0.Models
{
    [Table("Categories")]
    public class Category
    {
        public Category() { }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int  CategoryId { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(10)]
        public string CategoryName { get; set; }
       
        [Column(TypeName = "nvarchar")]
        [MaxLength(200)]
        public string Description { get; set; }

        public virtual ICollection<Product> Products { get; set; }  
    }
}