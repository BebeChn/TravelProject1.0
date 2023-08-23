using System;
using System.Collections.Generic;

namespace TravelProject1._0.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string? Gender { get; set; }

    public string? Address { get; set; }

    public string Phone { get; set; } = null!;

    public DateTime? Birthday { get; set; }

    public int? Age { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? Points { get; set; }

    public string PasswordHash { get; set; } = null!;

    public DateTime? CreateDate { get; set; }

    public bool EmailConfirmed { get; set; }

    public string Salt { get; set; } = null!;

    public string VerificationCode { get; set; } = null!;

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<QuestionReport> QuestionReports { get; set; } = new List<QuestionReport>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
