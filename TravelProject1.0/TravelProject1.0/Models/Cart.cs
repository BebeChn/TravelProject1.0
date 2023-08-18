using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelProject1._0.Models
{
    [Table("Cart")]
    public class Cart
    {
        
        public Cart() { }
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Product))]
        [Key]
        public int ProductId { get; set; }
   
        public int Quantity { get; set; }   
    }
}
