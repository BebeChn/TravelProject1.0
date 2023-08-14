using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TravelProject1._0.Models;

public partial class TravelUsersContext : DbContext
{
    public TravelUsersContext()
    {
    }

    public TravelUsersContext(DbContextOptions<TravelUsersContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot Config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsetting.json").Build();
            optionsBuilder.UseSqlServer(Config.GetConnectionString("TravelUsers"));
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6ED7B285354");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.ProductName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
