using System;
using System.Collections.Generic;

namespace TravelProject1._0.Models;

public partial class OrderDetail
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int PlanId { get; set; }

    public decimal? UnitPrice { get; set; }

    public short? Quantity { get; set; }

    public float? Discount { get; set; }

    public virtual Order IdNavigation { get; set; } = null!;

    public virtual Plan Plan { get; set; } = null!;
}
