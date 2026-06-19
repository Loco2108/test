using System.ComponentModel.DataAnnotations;

public class UserUsernameAvailabilityResponseDto
{
    public bool IsAvailable { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class UserUsernameCheckRequestDto
{
    [Required(ErrorMessage = "No username provided")]
    [RegularExpression("^[A-Za-z0-9]+$", ErrorMessage = "Username can only contain letters and digits")]
    [MaxLength(20, ErrorMessage = "Username is too long (max. 20 characters)")]
    public string Username { get; set; } = string.Empty;
}