using System;
using System.Collections.Generic;

namespace TravelProject1._0.Areas.Admin.Models;

public partial class Admin
{
    public int AdminId { get; set; }

    public string AdminName { get; set; } = null!;

    public string Account { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string Describe { get; set; } = null!;

    public DateTime? CreateDate { get; set; }

    public DateTime? LoginDate { get; set; }
}
