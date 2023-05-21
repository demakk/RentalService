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
    public DbSet<Country> Countries { get; set; }
    public DbSet<UserBasicInfo> UserBasicInfos { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItemLink> OrderItemLinks { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }   
}