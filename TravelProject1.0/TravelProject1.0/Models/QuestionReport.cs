using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelProject1._0.Models;
[Table("QuestionReport")]
public partial class QuestionReport
{
    public QuestionReport() { } 
    [Key]
    public int Id { get; set; }
    
    [ForeignKey("User")]
    public string UserId { get; set; } = null!;
    [Column(TypeName = "nvarchar")]
    [MaxLength(2000)]
    public string Describe { get; set; } = null!;

    public DateTime ReportDate { get; set; }

    public virtual User User { get; set; } = null!;

  
}
