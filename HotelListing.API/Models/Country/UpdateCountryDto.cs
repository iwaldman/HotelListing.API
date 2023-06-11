namespace HotelListing.API.Models.Country;

/// <summary>
/// UpdateCountryDto is a data transfer object (DTO) class for used for updating country information.
/// </summary>
public class UpdateCountryDto : BaseCountryDto
{
    public int Id { get; set; }
}
