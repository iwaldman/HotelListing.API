using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Models.Users;

public class ApiUserDto : LoginDto
{
    [Required]
    public required string FirstName { get; set; }

    [Required]
    public required string LastName { get; set; }
}
