namespace HotelListing.API.Models.Country;

/// <summary>
/// GetCountryDto is a data transfer object (DTO) class for retrieving country information.
/// </summary>
public class GetCountryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string ShortName { get; set; } = null!;
}
