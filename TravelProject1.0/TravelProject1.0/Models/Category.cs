using System;
using System.Collections.Generic;

namespace TravelProject1._0.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Describe { get; set; }
}
