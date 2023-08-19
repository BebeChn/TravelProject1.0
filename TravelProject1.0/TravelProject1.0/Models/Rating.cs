using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelProject1._0.Models;
[Table("Rating")]
public partial class Rating
{
    public Rating() { }
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    [Column(TypeName = "nvarchar")]
    [MaxLength(450)]
    public string UserId { get; set; } = null!;
    [ForeignKey("Product")]
    public int ProductId { get; set; }
    [Column(TypeName = "nvarchar")]
    [MaxLength(450)]
    public string RatingScore { get; set; } = null!;
    [Column(TypeName = "nvarchar")]
    [MaxLength(2000)]
    public string Describe { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
