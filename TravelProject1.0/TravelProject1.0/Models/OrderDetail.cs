using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelProject1._0.Models
{
    [Table("OrderDetails")]
    public class OrderDetail
    {
        public OrderDetail() { }    
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDetailId { get; set; }
        
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        
        [ForeignKey(nameof(Plan))]
        public int PlanId { get; set; }
        
        [Column(TypeName ="decimal(16,2)")]
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public float Discount { get; set; }

        public virtual Plan Plan { get; set; }
        public virtual Order Order { get; set; }
    }
}
