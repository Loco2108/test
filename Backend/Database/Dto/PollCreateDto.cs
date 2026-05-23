using System.ComponentModel.DataAnnotations;

public class PollCreateDto
{
    [Required]
    public string Question { get; set; } = string.Empty;
}