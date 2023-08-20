using System;
using System.Collections.Generic;

namespace TravelProject1._0.Models;

public partial class Cart
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ProductId { get; set; }

    public short? Quantity { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
