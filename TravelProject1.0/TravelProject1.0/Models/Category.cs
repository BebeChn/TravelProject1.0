using System;
using System.Collections.Generic;

namespace TravelProject1._0.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Describe { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
