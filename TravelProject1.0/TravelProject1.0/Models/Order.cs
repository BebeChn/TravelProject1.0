using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelProject1._0.Models;
[Table("Order")]
public partial class Order
{
    public Order() { }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderId { get; set; }
    [Column(TypeName = "nvarchar")]
    [MaxLength(450)]
    public string Id { get; set; } = null!;
  
    public DateTime OrderDate { get; set; }

    public virtual User IdNavigation { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } 
}
