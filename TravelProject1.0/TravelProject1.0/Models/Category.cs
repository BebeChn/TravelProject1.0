using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelProject1._0.Models;

[Table("Categories")]
public partial class Category
{
    public Category() { }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Column(TypeName = "nvarchar")]
    [MaxLength(50)]
    public string Name { get; set; } = null!;
    [Column(TypeName = "nvarchar")]
    [MaxLength(2000)]
    public string Description { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
