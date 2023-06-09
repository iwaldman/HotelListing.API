namespace HotelListing.API.Data;

/// <summary>
/// Represents a country.
/// </summary>
public class Country
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string ShortName { get; set; }
    public virtual IList<Hotel>? Hotels { get; set; }
}