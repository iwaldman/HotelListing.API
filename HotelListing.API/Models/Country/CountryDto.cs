using HotelListing.API.Models.Hotel;

namespace HotelListing.API.Models.Country;

public class CountryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string ShortName { get; set; } = null!;

    public List<HotelDto>? Hotels { get; set; }
}
