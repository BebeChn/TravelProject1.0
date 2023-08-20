using System;
using System.Collections.Generic;

namespace TravelProject1._0.Models;

public partial class Rating
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ProductId { get; set; }

    public short RatingScore { get; set; }

    public string? Describe { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
