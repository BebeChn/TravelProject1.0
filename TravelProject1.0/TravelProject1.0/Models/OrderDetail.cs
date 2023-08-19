using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelProject1._0.Models;
[Table("OrderDetails")]
public partial class OrderDetail
{
    public OrderDetail() { }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
    public int Id { get; set; }
    [ForeignKey("Order")]
    public int OrderId { get; set; }
    [ForeignKey("Plan")]
    public int PlanId { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public float Discount { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Plan Plan { get; set; } = null!;
}
