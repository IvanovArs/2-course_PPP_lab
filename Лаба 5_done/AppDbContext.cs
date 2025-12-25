using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
namespace Лаба_4
{
    public class AppDbContext : DbContext{
        public DbSet<ClientDbModel> Clients { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "clients.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<ClientDbModel>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<ClientDbModel>()
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            modelBuilder.Entity<ClientDbModel>()
                .Property(e => e.ClientType)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<ClientDbModel>()
                .Property(e => e.PricingStrategyType)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<ClientDbModel>()
                .Property(e => e.AdditionalInfo)
                .HasMaxLength(500);
        }
    }
    public class ClientDbModel{
        public int Id { get; set; }
        public string Name { get; set; }
        public string ClientType { get; set; }
        public double BaseCost { get; set; }
        public string PricingStrategyType { get; set; }
        public double DiscountValue { get; set; }
        public string AdditionalInfo { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}