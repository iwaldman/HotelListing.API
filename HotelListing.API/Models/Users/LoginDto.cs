using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Models.Users;

public class LoginDto
{
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
