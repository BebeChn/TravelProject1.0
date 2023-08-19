using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TravelProject1._0.Models;

public partial class TravelUserContext : IdentityDbContext<User, Role, string, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
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
        modelBuilder.Entity<Category>().HasData(new Category { Id = 1, Name = "機票票卷", Description = "飛機" });
        modelBuilder.Entity<Category>().HasData(new Category { Id = 2, Name = "住宿票卷", Description = "住宿" });
        modelBuilder.Entity<Category>().HasData(new Category { Id = 3, Name = "交通票卷", Description = "交通" });
        modelBuilder.Entity<Category>().HasData(new Category { Id = 4, Name = "景點票卷", Description = "景點" });



        OnModelCreatingPartial(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
