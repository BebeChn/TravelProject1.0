using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelProject1._0.Models;
[Table("Products")]
public partial class Product
{
    public Product() { }
    [Key]
    public int Id { get; set; }

    [ForeignKey("Category")]
    public int CategoryId { get; set; }
    [Column(TypeName = "nvarchar")]
    [MaxLength(450)]
    public string Name { get; set; } = null!;

    public decimal Price { get; set; }
    [Column(TypeName = "nvarchar")]
    [MaxLength(2000)]
    public string? MainDescription { get; set; }
    [Column(TypeName = "nvarchar")]
    [MaxLength(2000)]
    public string? SubDescription { get; set; }
    [Column(TypeName = "nvarchar")]
    [MaxLength(2000)]
    public string? ShortDescription { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Plan> Plans { get; set; } = new List<Plan>();
}
