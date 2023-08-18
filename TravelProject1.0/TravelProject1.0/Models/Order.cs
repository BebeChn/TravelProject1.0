using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelProject1._0.Models
{
    [Table("Order")]
    public class Order
    {
        public Order() { }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        
        public int Id { get; set; }


        public DateTime OrderDate { get; set; }

        public DateTime ShippedDate  { get; set; }
    }
}