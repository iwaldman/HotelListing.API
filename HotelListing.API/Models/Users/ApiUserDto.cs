using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Models.Users;

public class ApiUserDto
{
    [Required]
    public required string FirstName { get; set; }

    [Required]
    public required string LastName { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [StringLength(
        15,
        ErrorMessage = "Password is limited to {2}-{1} characters",
        MinimumLength = 6
    )]
    public required string Password { get; set; }
}
