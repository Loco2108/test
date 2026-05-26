using System.ComponentModel.DataAnnotations;

namespace Backend.Dto;

public class UserRegisterDto
{
    [Required]
    [MaxLength(50)]
    public required string Username { get; set; }

    [Required]
    public required string Password { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public required string Email { get; set; }
}