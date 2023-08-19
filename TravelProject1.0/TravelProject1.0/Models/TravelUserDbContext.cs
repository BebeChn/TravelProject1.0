using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TravelProject1._0.Models
{
    public class TravelUserDbContext : IdentityDbContext
    {
       
        public TravelUserDbContext() { }
        public TravelUserDbContext(DbContextOptions<TravelUserDbContext> options)
           : base(options)
        {
        }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Category { get; set; }

        public DbSet<Order> Order { get; set; }

        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Plan> Plan { get; set; }

        public DbSet<PlanCalendar> PlanCalendar { get; set; }

        public DbSet<PlanOption> PlanOption { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<QuestionReport> QuestionReport { get; set; }

        public DbSet<Rating> Rating { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot Config = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json").Build();
                optionsBuilder.UseSqlServer(Config.GetConnectionString("DefaultConnection"));
            }
        }
    }
}
