using System;
using System.Collections.Generic;

namespace TravelProject1._0.Models;

public partial class Plan
{
    public int PlanId { get; set; }

    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string? Describe { get; set; }

    public string? PlanImg { get; set; }

    public decimal PlanPrice { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<PlanCalendar> PlanCalendars { get; set; } = new List<PlanCalendar>();

    public virtual Product Product { get; set; } = null!;
}
