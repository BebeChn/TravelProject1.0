using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TravelProject1._0.Models;

public partial class TravelUserContext : IdentityDbContext<User,Role,string,UserClaim,UserRole,UserLogin,RoleClaim,UserToken>
{
    public TravelUserContext()
    {
    }

    public TravelUserContext(DbContextOptions<TravelUserContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Plan> Plans { get; set; }

    public virtual DbSet<PlanCalendar> PlanCalendars { get; set; }


    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<QuestionReport> QuestionReports { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Role> Role { get; set; }
    public virtual DbSet<User> User { get; set; }
    public virtual DbSet<UserClaim> UserClaim { get; set; }

    public virtual DbSet<UserLogin> UserLogin { get; set; }

    public virtual DbSet<UserRole> UserRole { get; set; }

    public virtual DbSet<UserToken> UserToken { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        OnModelCreatingPartial(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
