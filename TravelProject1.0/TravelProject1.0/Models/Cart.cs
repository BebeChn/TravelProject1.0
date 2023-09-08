using System;
using System.Collections.Generic;

namespace TravelProject1._0.Models;

public partial class Cart
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int PlanId { get; set; }

    public short? CartQuantity { get; set; }

    public decimal? CartPrice { get; set; }

    public string? CartName { get; set; }

    public DateTime? CartDate { get; set; }

    public virtual Plan Plan { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
