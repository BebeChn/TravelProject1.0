using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelProject1._0.Models;
[Table("Plan")]
public partial class Plan
{
    public Plan() { }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey("Product")]
    public int ProductId { get; set; }
    [Column(TypeName = "nvarchar")]
    [MaxLength(450)]
    public string Name { get; set; } = null!;
    [Column(TypeName = "nvarchar")]
    [MaxLength(2000)]
    public string MainDescription { get; set; } = null!;

  

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Product Product { get; set; } = null!;
}
