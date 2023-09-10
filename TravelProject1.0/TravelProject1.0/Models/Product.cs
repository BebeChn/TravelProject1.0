using System;
using System.Collections.Generic;

namespace TravelProject1._0.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public int Id { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal Price { get; set; }

    public string? MainDescribe { get; set; }

    public string? SubDescribe { get; set; }

    public string? ShortDescribe { get; set; }

    public string? Img { get; set; }

    public string? Longitude { get; set; }

    public string? Latitude { get; set; }

    public virtual ICollection<CollectTable> CollectTables { get; set; } = new List<CollectTable>();

    public virtual Category IdNavigation { get; set; } = null!;

    public virtual ICollection<Plan> Plans { get; set; } = new List<Plan>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
