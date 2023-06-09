using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListing.API.Data;

/// <summary>
/// Represents a hotel.
/// </summary>
public class Hotel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public double Rating { get; set; }

    [ForeignKey(nameof(Country))]
    public int CountryId { get; set; }
    public Country? Country { get; set; }
}
