namespace Backend.Dto;

public class UserLoginDto
{
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool StaySignedIn { get; set; } = false;
}