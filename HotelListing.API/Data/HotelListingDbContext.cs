using HotelListing.API.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Data;

/// <summary>
/// Represents a database context for hotel listings.
/// </summary>
public class HotelListingDbContext : IdentityDbContext<ApiUser>
{
    public HotelListingDbContext(DbContextOptions options)
        : base(options) { }

    public DbSet<Country>? Countries { get; set; }
    public DbSet<Hotel>? Hotels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .ApplyConfiguration(new RoleConfiguration())
            .ApplyConfiguration(new CountryConfiguration())
            .ApplyConfiguration(new HotelConfiguration());
    }
}
