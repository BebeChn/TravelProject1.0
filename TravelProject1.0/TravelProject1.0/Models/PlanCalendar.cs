using System;
using System.Collections.Generic;

namespace TravelProject1._0.Models;

public partial class PlanCalendar
{
    public int Id { get; set; }

    public int PlanId { get; set; }

    public DateTime? Date { get; set; }

    public decimal Price { get; set; }

    public virtual Plan Plan { get; set; } = null!;
}
