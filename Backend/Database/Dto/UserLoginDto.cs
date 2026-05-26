using System.ComponentModel.DataAnnotations;

namespace Backend.Dto;

public class UserLoginDto
{
    [Required]
    public required string Password { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public required string Email { get; set; }

    public bool StaySignedIn { get; set; } = false;
}