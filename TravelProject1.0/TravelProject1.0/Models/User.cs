﻿using System;
using System.Collections.Generic;

namespace TravelProject1._0.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string? Gender { get; set; }

    public string Phone { get; set; } = null!;

    public DateTime? Birthday { get; set; }

    public int? Age { get; set; }

    public string Email { get; set; } = null!;

    public int? Points { get; set; }

    public string PasswordHash { get; set; } = null!;

    public DateTime? CreateDate { get; set; }

    public string? Salt { get; set; }

    public string? VerificationCode { get; set; }

    public string? ResetToken { get; set; }

    public int? RoleId { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual Role? Role { get; set; }
}
