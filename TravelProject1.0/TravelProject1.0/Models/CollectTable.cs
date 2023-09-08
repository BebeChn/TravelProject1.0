using System;
using System.Collections.Generic;

namespace TravelProject1._0.Models;

public partial class CollectTable
{
    public int CollectId { get; set; }

    public int ProductId { get; set; }

    public int UserId { get; set; }

    public string? CollectName { get; set; }

    public string? CollectImage { get; set; }

    public virtual Product Product { get; set; } = null!;
}
