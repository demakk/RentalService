using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RentalService.Domain.Aggregates.Common;
using RentalService.Domain.Aggregates.ItemAggregates;
using RentalService.Domain.Aggregates.OrderAggregates;
using RentalService.Domain.Aggregates.ShoppingCartAggregates;
using RentalService.Domain.Aggregates.UserProfileAggregates;

namespace RentalService.Dal;

public class DataContext : IdentityDbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
        
    }
    public DbSet<UserBasicInfo> UserBasicInfos { get; set; }

    public DbSet<Item> Items { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItemLink> OrderItemLinks { get; set; }

    public DbSet<Manager> Managers { get; set; }

    public DbSet<ItemCategory> ItemCategories { get; set; }
    public DbSet<Manufacturer> Manufacturers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<OrderItemLink>().HasKey(ol => new { ol.ItemId, ol.OrderId});
    }   
}