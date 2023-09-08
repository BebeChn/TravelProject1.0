using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TravelProject1._0.Models;

public partial class TravelProjectAzureContext : DbContext
{
    public TravelProjectAzureContext()
    {
    }

    public TravelProjectAzureContext(DbContextOptions<TravelProjectAzureContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CollectTable> CollectTables { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Plan> Plans { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<QuestionReport> QuestionReports { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<ResetPasswordToken> ResetPasswordTokens { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VerificationCode> VerificationCodes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot Config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings").Build();
            optionsBuilder.UseSqlServer(Config.GetConnectionString("TravelProject"));
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Chinese_Taiwan_Stroke_CI_AS");

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Admin__3214EC277CEED0A0");

            entity.ToTable("Admin");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Account).HasMaxLength(50);
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Describe).HasMaxLength(200);
            entity.Property(e => e.LoginDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Role).HasMaxLength(10);

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Admins)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Admin_Role");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cart__3214EC278420B13A");

            entity.ToTable("Cart");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CartDate).HasColumnType("date");
            entity.Property(e => e.CartPrice).HasColumnType("money");
            entity.Property(e => e.PlanId).HasColumnName("PlanID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Plan).WithMany(p => p.Carts)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Plan");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cart__UserID__4D94879B");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC27904EA672");

            entity.ToTable("Category");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Describe).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(10);
        });

        modelBuilder.Entity<CollectTable>(entity =>
        {
            entity.HasKey(e => e.CollectId);

            entity.ToTable("CollectTable");

            entity.Property(e => e.CollectId).HasColumnName("CollectID");
            entity.Property(e => e.CollectImage).HasMaxLength(50);
            entity.Property(e => e.CollectName).HasMaxLength(50);
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Product).WithMany(p => p.CollectTables)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CollectTable_Products");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAFF4E1F948");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.OrderDate).HasColumnType("date");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__UserID__5070F446");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order De__3214EC278802C910");

            entity.ToTable("Order Detail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Odimg).HasColumnName("ODImg");
            entity.Property(e => e.Odname).HasColumnName("ODname");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.PlanId).HasColumnName("PlanID");
            entity.Property(e => e.UnitPrice).HasColumnType("money");
            entity.Property(e => e.UseDate).HasColumnType("date");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order Detail_Orders");

            entity.HasOne(d => d.Plan).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order Detail_Plan1");
        });

        modelBuilder.Entity<Plan>(entity =>
        {
            entity.HasKey(e => e.PlanId).HasName("PK__Plan__755C22D754D16B20");

            entity.ToTable("Plan");

            entity.Property(e => e.PlanId).HasColumnName("PlanID");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PlanPrice).HasColumnType("money");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Product).WithMany(p => p.Plans)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Plan_Products");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6EDF2375538");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Latitude).HasMaxLength(50);
            entity.Property(e => e.Longitude).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.ProductName).HasMaxLength(50);

            entity.HasOne(d => d.IdNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__ID__787EE5A0");
        });

        modelBuilder.Entity<QuestionReport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3214EC27FEDEB8D2");

            entity.ToTable("Question Report");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Describe).HasMaxLength(200);
            entity.Property(e => e.ReportDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.QuestionReports)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Question __UserI__5441852A");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rating__3214EC277B4AEC0F");

            entity.ToTable("Rating");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Describe).HasMaxLength(200);
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.RatingDate).HasColumnType("date");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Product).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rating__ProductI__7A672E12");

            entity.HasOne(d => d.User).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rating__UserID__5629CD9C");
        });

        modelBuilder.Entity<ResetPasswordToken>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ResetPasswordToken");

            entity.Property(e => e.ExpireTime).HasColumnType("datetime");
            entity.Property(e => e.HashedToken)
                .HasMaxLength(50)
                .IsFixedLength();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.RoleId).ValueGeneratedNever();
            entity.Property(e => e.RoleName).HasMaxLength(10);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCACFCC705CB");

            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Age).HasComputedColumnSql("(datediff(year,[Birthday],getdate()))", false);
            entity.Property(e => e.Birthday).HasColumnType("date");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.Phone).HasMaxLength(20);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_User_Role");
        });

        modelBuilder.Entity<VerificationCode>(entity =>
        {
            entity.HasKey(e => e.VerificationId);

            entity.ToTable("VerificationCode");

            entity.Property(e => e.ExpiryTime).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
