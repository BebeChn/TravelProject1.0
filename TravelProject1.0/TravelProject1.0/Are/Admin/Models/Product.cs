using System;
using System.Collections.Generic;

namespace TravelProject1._0.Are.Admin.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public int CategoryId { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal Price { get; set; }

    public string? Describe { get; set; }

    public string? Describe1 { get; set; }

    public string? Describe2 { get; set; }
}
