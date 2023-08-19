using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelProject1._0.Models;
[Table("Admin")]
public partial class Admin
{
    public Admin() { }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column(TypeName ="nvarchar")]
    [MaxLength(50)]
    public string Name { get; set; } = null!;
    [Column(TypeName = "nvarchar")]
    [MaxLength(30)]
    public string Account { get; set; } = null!;
    [Column(TypeName = "nvarchar")]
    [MaxLength(20)]
    public string Password { get; set; } = null!;
    [Column(TypeName = "nvarchar")]
    [MaxLength(10)]
    public string Role { get; set; } = null!;
    [Column(TypeName = "nvarchar")]
    [MaxLength(2000)]
    public string Description { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public DateTime LoginDate { get; set; }
}
