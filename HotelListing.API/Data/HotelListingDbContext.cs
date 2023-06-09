﻿using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Data;

/// <summary>
/// Represents a database context for hotel listings.
/// </summary>
public class HotelListingDbContext : DbContext
{
    public HotelListingDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Country>? Countries { get; set; }
    public DbSet<Hotel>? Hotels { get; set; }
}
