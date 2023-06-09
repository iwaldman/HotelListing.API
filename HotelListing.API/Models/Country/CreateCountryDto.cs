using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Models.Country;

public class CreateCountryDto
{
    [Required]
    [StringLength(maximumLength: 50, ErrorMessage = "Country name is too long")]
    public required string Name { get; set; }

    [Required]
    [StringLength(maximumLength: 2, ErrorMessage = "Short country name is too long")]
    public required string ShortName { get; set; }
}
