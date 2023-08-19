using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelProject1._0.Models;
[Table("Cart")]
public partial class Cart
{
    public Cart() { }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [MaxLength(50)]
    public int Id { get; set; }

    [ForeignKey("Product")]
    public int ProductId { get; set; }
    [Column(TypeName = "nvarchar")]
    [MaxLength(450)]
    [ForeignKey("User")]
    public string UserId { get; set; }

    public int Quantity { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
