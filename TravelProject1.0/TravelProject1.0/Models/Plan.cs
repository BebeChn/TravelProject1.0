using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelProject1._0.Models
{
    [Table("Plans")]
    public class Plan
    {
        public Plan() { }
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int  PlanId { get; set; }
        
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        
        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string PlanName { get; set; }
        
        [Column(TypeName = "nvarchar")]
        [MaxLength(2000)]
        public string PlanDescription { get; set; }
       
        [Column(TypeName = "nvarchar")]
        [MaxLength(2000)]
        public string PlanDescription2 { get; set; }
        
        [Column(TypeName = "nvarchar")]
        [MaxLength(2000)]
        public string PlanDescription3 { get; set; }

        public virtual Product Product { get; set; }
         
    }
}