using System;
using System.Collections.Generic;

namespace TravelProject1._0.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string? Gender { get; set; }

    public string? Address { get; set; }

    public string Phone { get; set; } = null!;

    public DateTime? Birthday { get; set; }

    public int? Age { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? Points { get; set; }

    public bool? Notify { get; set; }

    public DateTime? CreateDate { get; set; }
}
