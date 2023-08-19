using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelProject1._0.Models;
[Table("Planalendar")]
public partial class PlanCalendar
{
   
    public PlanCalendar() { }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey("Plan")]
    public int PlanId { get; set; }

    public DateTime Date { get; set; }

    public decimal Price { get; set; }

    public virtual Plan Plan { get; set; } = null!;
}
