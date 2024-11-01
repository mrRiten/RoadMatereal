using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RoadMatereal.Models
{
    public class RoadMaterialContext : IdentityDbContext<ApplicationUser, Role, int>
    {
        public RoadMaterialContext(DbContextOptions<RoadMaterialContext> options) : base(options) { }

        // DbSets for other entities
        public DbSet<Order> Orders { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Client)
                .WithMany()
                .HasForeignKey(o => o.ClientID);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Status)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.StatusID);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderID);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Material)
                .WithMany(m => m.OrderItems)
                .HasForeignKey(oi => oi.MaterialID);

            modelBuilder.Entity<Material>()
                .HasOne(m => m.Supplier)
                .WithMany(s => s.Materials)
                .HasForeignKey(m => m.SupplierID);
        }
    }
}