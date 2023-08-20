using System;
using System.Collections.Generic;

namespace TravelProject1._0.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string? MainDescribe { get; set; }

    public string? SubDescribe { get; set; }

    public string? ShortDescribe { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Category IdNavigation { get; set; } = null!;

    public virtual ICollection<Plan> Plans { get; set; } = new List<Plan>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
