using System;
using System.Collections.Generic;

namespace TravelProject1._0.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public DateTime? OrderDate { get; set; }

    public virtual OrderDetail? OrderDetail { get; set; }

    public virtual User User { get; set; } = null!;
}
