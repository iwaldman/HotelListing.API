using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Models.Country;

/// <summary>
/// BaseCountryDto is a data transfer object (DTO) class for used for creating or updating country information.
/// </summary>
public abstract class BaseCountryDto
{
    [Required]
    [StringLength(maximumLength: 50, ErrorMessage = "Country name is too long")]
    public required string Name { get; set; }

    [Required]
    [StringLength(maximumLength: 2, ErrorMessage = "Short country name is too long")]
    public required string ShortName { get; set; }
}
