using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelProject1._0.Models
{
    [Table("Cart")]
    [PrimaryKey(nameof(ProductId),(nameof(Id)),nameof(ShoppingCartID))]
    public class Cart
    {
        
        public Cart() { }

        [ForeignKey(nameof(IdentityUser))]
        [Column(TypeName = "nvarchar")]
        [MaxLength(450)]
        public string Id { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        
        public int ShoppingCartID { get; set; }
   
        public int Quantity { get; set; }   

        public virtual IdentityUser IdentityUser { get; set; }

        public virtual Product Product { get; set; }
    }
}
